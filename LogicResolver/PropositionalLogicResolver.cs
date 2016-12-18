using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicResolver
{

    class PropositionalLogicResolver
    {
        Dictionary<string, List<bool>> varValues;
        List<bool>[] output;

        //and or ifthen not biconditional operators 
        static readonly string opsLibrary = "&?-~=";

        
        #region Resolve Methods
        public Result Resolve(string toResolve)
        {
            //Do initial checks and preprocessing
            toResolve = addOuterMostParansIfNotPresent(toResolve);
            toResolve = NegatedVariableAutoFix(toResolve);

            output = new List<bool>[toResolve.Length];
            varValues = getVariableTruthValues(toResolve);

            //resolve function
            Result returnVal = Resolve(toResolve,0,new List<bool>());

            Console.WriteLine("The Expression: " + toResolve + " evaluates to: ");

            bool isTautology = true;

            foreach (bool b in returnVal.curval) {

                if (b == true) 
                Console.Write("T ");

                if (b == false)
                {
                    Console.Write("F ");
                    isTautology = false;
                }
            }
            if (isTautology) { Console.Write(" and is a Tautology"); } else { Console.Write(" and is not a Tautology"); }

            Console.WriteLine();
            PrintOutput(toResolve);

            return returnVal;
        }

        public Result Resolve(string toResolve, int index, List<bool> curval) 
        {
            int keepsync;
            int charAt = index;
            Result returnVal = new Result();
            for (int i = 0; i < toResolve.Length; i++)
            {
                //to keep in sync if charAt is updated to ensure if's below don't look at next char
                if (charAt == toResolve.Length) return new Result { charAt = charAt - 1, curval = curval, toResolve = toResolve };

                keepsync = charAt;
                if (toResolve.ElementAt(keepsync) == '(')
                {
                    charAt++; 
                }
                if (toResolve.ElementAt(keepsync) == ')')
                {                    
                    charAt++;
                    return new Result(toResolve, curval, charAt);
                }
                if (opsLibrary.Contains(toResolve.ElementAt(keepsync)))
                {
                                                                      //get op          //val 1   // val 2
                    Result res = SelectAndExecuteOperation(toResolve.ElementAt(charAt), curval, Resolve(toResolve, charAt+1, new List<bool>()));
                    output[charAt] = res.curval;//print to output
                    curval = res.curval;//set curval
                    charAt = res.charAt;
                  
                }
                if (IsEnglishLetter(toResolve.ElementAt(keepsync))) 
                {
                    string varName = toResolve.ElementAt(charAt).ToString();
                    output[charAt] = varValues[varName];//print out
                    curval = varValues[varName];//set cur val
                    charAt++; 
                }
                

            }

            //should return somewhere in the function not out here
            //this is just here to make the compiler happy
                return new Result();
        }//end of result

        #endregion

        #region Parsing

        public Dictionary<string, List<bool>> getVariableTruthValues(String expression)
        {
            Dictionary<string, List<bool>> values = new Dictionary<string, List<bool>>();
            List<string> vars = new List<string>();

            foreach (char s in expression) {

                if (IsEnglishLetter(s) & !vars.Contains(s.ToString())) {
                    vars.Add(s.ToString());
                }
            }
            //alphabetical order
            vars.Sort();

            //The number of rows in a truth table is 2^(number of variables)
            int numTF = (int)Math.Pow(2, vars.Count); 
            List<bool> tvalues;
            for (int i = 0; i < vars.Count; i++)
            {
                //key value pair that will be stored
                //in dictionary
                tvalues = new List<bool>();
                string key = vars.ElementAt(i);
                //ie 2 4 8 16
                bool toWrite = true;

                for (int j = 0; j < numTF; j++)//write tf
                {
                    if ((j % (numTF / Math.Pow(2, i + 1))) == 0 & j !=0 ) { toWrite = !toWrite; }
                    tvalues.Add(toWrite);

                }//end for

                //put values for variable in dictionary
                if(!values.ContainsKey(vars.ElementAt(i)))
                values.Add(vars.ElementAt(i), tvalues);
            }
                return values;
        }

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
                            if (IsEnglishLetter(modify[count + 1]))
                            {
                                char letter = modify[count + 1];
                               modify = modify.Remove(count + 1,1);
                               modify = modify.Insert(count + 1, "(" + letter + ")");
                            }
                        }
                }
                count++;
                if (count >= modify.Length) { 
                    cont = false; }
            }
            return modify;
        }

        public string addOuterMostParansIfNotPresent(string input)
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
        #endregion

        #region Operators
        public List<bool> orOperation(List<bool> op1, List<bool> op2)
        {

            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++) 
            {
                //add the result of Or'ing the operands
                ResultList.Add(op1.ElementAt(i) || op2.ElementAt(i));
            }
                return ResultList;
        }

        public List<bool> andOperation(List<bool> op1, List<bool> op2)
        {

            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++)
            {
                //add the result of And'ing the operands
                ResultList.Add(op1.ElementAt(i) && op2.ElementAt(i));
            }
            return ResultList;
        }
        public List<bool> ifthenOperation(List<bool> op1, List<bool> op2)
        {

            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++)
            {
                //if only false when T->F
                if (op1.ElementAt(i) & !op2.ElementAt(i))
                {
                    ResultList.Add(false);
                }
                else
                {
                    ResultList.Add(true);
                }
            }
            return ResultList;
        }
        public List<bool> notOperation(List<bool> op1)
        {

            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++)
            {
                //add the result of not'ing the operand
                ResultList.Add(!op1.ElementAt(i));
            }
            return ResultList;
        }
        public List<bool> BiConditionalOperation(List<bool> op1, List<bool> op2)
        {
            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++)
            {
                //add the result of bicond'ing the operand
                ResultList.Add(op1.ElementAt(i) == op2.ElementAt(i));
            }

            return ResultList;
        }

        public bool IsEnglishLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        public Result SelectAndExecuteOperation(char op, List<bool> curval, Result res)
        {
            Result toReturn = new Result();
            if (op == '&') { toReturn.curval = andOperation(curval, res.curval); }
            if (op == '?') { toReturn.curval = orOperation(curval, res.curval); }
            if (op == '-') { toReturn.curval = ifthenOperation(curval, res.curval); }
            if (op == '~') { toReturn.curval = notOperation(res.curval); }
            if (op == '=') { toReturn.curval = BiConditionalOperation(curval, res.curval); }
            toReturn.charAt = res.charAt;
            toReturn.toResolve = res.toResolve;
            return toReturn;
        }
        #endregion

        #region Printing
        public int getNumOfUniqueLettersInString(string toResolve)
        {
            List<char> alreadyCounted = new List<char>();
            foreach (char c in toResolve)
            {
                if (IsEnglishLetter(c))
                {
                    if (!alreadyCounted.Contains(c)) 
                    {
                        alreadyCounted.Add(c);
                    }                  
                }
            }
            return alreadyCounted.Count;
        }


        public void PrintOutput(string toResolve)
        {
            char[] chars = toResolve.ToCharArray();
            foreach (char c in chars)
            {
                Console.Write(c);
                Console.Write(" ");
            }
            Console.WriteLine();
            Console.WriteLine();
            //now have header fields

            for (int j = 0; j < Math.Pow(2, getNumOfUniqueLettersInString(toResolve)); j++)
            {
                for (int i = 0; i < output.Length; i++)
                {
                    if(output[i] == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                    
                        if (output[i].Count == 0) {
                            Console.Write(" ");
                        }
                        else
                        {
                          if (output[i].ElementAt(j) == true) { Console.Write("T"); } else { Console.Write("F"); }
                        }
                    }
                    Console.Write(" ");

                }//end of inner for
                Console.WriteLine();
            }//end of outer for

        }

        #endregion

        #region ValidateInput

        public string ProperSyntax(string input)
        {
            string isProper = "true";
            for (int i = 0; i < input.Length - 1; i++)
            {
                char c = input[i];
                if (opsLibrary.Contains(c))
                {
                    if (c == '~')// ( 
                    {
                        if (!("(~".Contains(input[i + 1]) || IsEnglishLetter(input[i + 1]))) { isProper = "Invalid Character following ~ at position " + (i+1) +" in input string"; break; }
                        
                    } 
                    else // ( var,~
                    {
                        if (!("(~".Contains(input[i + 1]) || IsEnglishLetter(input[i + 1]))) { isProper = "Invalid Character following "+ c +" at position " + (i+1) + " in input string"; break; }
                    } 
                }
                if (IsEnglishLetter(c))// ), operator
                {
                    if (!(")&?-=".Contains(input[i + 1]))) { isProper = "Invalid Character following " + c + " at position " + (i+1) + " in input string"; break; }
                }
                if (c == ')') // (, ), &?-
                {
                    if (!("()&?-=".Contains(input[i + 1]))) { isProper = "Invalid Character following " + c + " at position " + (i+1) + " in input string"; break; }
                }
                if (c == '(') // var (~
                {
                    if (!("(~".Contains(input[i + 1]) || IsEnglishLetter(input[i + 1]))) { isProper = "Invalid Character following " + c + " at position " + (i+1) + " in input string"; break; }
                }
            }
            return isProper;
        }
        
       

        public string CheckBalancedParans(string input)
        {
            string isBalanced = "true";
            int LparanCount =0;
            int RparanCount =0;
            foreach (char c in input)
            { 
                if(c == '(')  { LparanCount++; }
                if (c == ')') { RparanCount++; }
            }


            if (!(LparanCount == RparanCount)) isBalanced = "Parantheses aren't balanced.";

            return isBalanced;
        }



        #endregion
    }
}


