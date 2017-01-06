using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicResolver
{
    public class LogicResolverInputValidator
    {

        // This defines a defacto Quasi-BNF for the language, although there isn't enforcement of parans ie a-b-c is valid still
        public string CheckSyntax(string input)
        {
            string isProper = "true";
            for (int i = 0; i < input.Length - 1; i++)
            {
                char c = input[i];
                if (LogicResolverConstants.OpsLibrary.Contains(c))
                {
                    if (c == '~')// ( 
                    {
                        if (!("(~".Contains(input[i + 1]) || StringUtilities.IsEnglishLetter(input[i + 1]))) { isProper = "Invalid Character following ~ at position " + (i + 1) + " in input string"; break; }

                    }
                    else // ( var,~
                    {
                        if (!("(~".Contains(input[i + 1]) || StringUtilities.IsEnglishLetter(input[i + 1]))) { isProper = "Invalid Character following " + c + " at position " + (i + 1) + " in input string"; break; }
                    }
                }
                if (StringUtilities.IsEnglishLetter(c))// ), operator
                {
                    if (!(")&?-=".Contains(input[i + 1]))) { isProper = "Invalid Character following " + c + " at position " + (i + 1) + " in input string"; break; }
                }
                if (c == ')') // (, ), &?-
                {
                    if (!("()&?-=".Contains(input[i + 1]))) { isProper = "Invalid Character following " + c + " at position " + (i + 1) + " in input string"; break; }
                }
                if (c == '(') // var (~
                {
                    if (!("(~".Contains(input[i + 1]) || StringUtilities.IsEnglishLetter(input[i + 1]))) { isProper = "Invalid Character following " + c + " at position " + (i + 1) + " in input string"; break; }
                }
            }
            return isProper;
        }

        public string CheckBalancedParans(string input)
        {
            string isBalanced = "true";
            int LparanCount = 0;
            int RparanCount = 0;
            foreach (char c in input)
            {
                if (c == '(') { LparanCount++; }
                if (c == ')') { RparanCount++; }
            }


            if (!(LparanCount == RparanCount)) isBalanced = "Parantheses aren't balanced.";

            return isBalanced;
        }

        //Because negation ie ~ doesn't take 2 parameters like other variables this fix is used to properly scope the effects of ~
        public string NegatedVariableAutoFix(string input)
        {
            string modify = input;
            bool cont = true;
            int count = 0;
            while (cont)
            {
                if (modify[count] == '~')
                {
                    if (modify.Length > count + 1)
                    {
                        if (StringUtilities.IsEnglishLetter(modify[count + 1]))
                        {
                            char letter = modify[count + 1];
                            modify = modify.Remove(count + 1, 1);
                            modify = modify.Insert(count + 1, "(" + letter + ")");
                        }
                    }
                }
                count++;
                if (count >= modify.Length)
                {
                    cont = false;
                }
            }
            return modify;
        }

        // Just for prettiness
        public string AddOuterMostParansIfNotPresent(string input)
        {
            if (input[0] != '(')
            {
                input = "(" + input + ")";
            }
            else if (input[input.Length - 1] != ')')
            {
                input = "(" + input + ")";
            }
            return input;
        }
    }
}
