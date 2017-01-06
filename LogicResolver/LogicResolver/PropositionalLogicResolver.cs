using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicResolver
{

    public class PropositionalLogicResolver
    {
        private Dictionary<string, List<bool>> varValues;
        private List<bool>[] output;
        private ResultPrinter printer = new ResultPrinter();
        private LogicResolverInputParser inputParser = new LogicResolverInputParser();
        private LogicResolverInputValidator validator = new LogicResolverInputValidator();


        public Result Resolve(string toResolve)
        {
            //Do initial checks and preprocessing
            toResolve = validator.AddOuterMostParansIfNotPresent(toResolve);
            toResolve = validator.NegatedVariableAutoFix(toResolve);

            output = new List<bool>[toResolve.Length];
            varValues = inputParser.GetVariableTruthValues(toResolve);

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
            printer.PrintOutput(toResolve, output);

            return returnVal;
        }

        private Result Resolve(string toResolve, int index, List<bool> curval) 
        {
            int keepsync;
            int charAt = index;
            Result returnVal = new Result();
            for (int i = 0; i < toResolve.Length; i++)
            {
                if (charAt == toResolve.Length)
                {
                    return new Result()
                    {
                        charAt = charAt - 1,
                        curval = curval,
                        toResolve = toResolve
                    };
                }

                //to keep in sync if charAt is updated to ensure if's below don't look at next char
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

                if (LogicResolverConstants.OpsLibrary.Contains(toResolve.ElementAt(keepsync)))
                {
                                                                      //get op          //val 1   // val 2
                    Result res = SelectAndExecuteOperation(toResolve.ElementAt(charAt), curval, Resolve(toResolve, charAt+1, new List<bool>()));
                    output[charAt] = res.curval;//print to output
                    curval = res.curval;//set curval
                    charAt = res.charAt;                 
                }

                if (StringUtilities.IsEnglishLetter(toResolve.ElementAt(keepsync))) 
                {
                    string varName = toResolve.ElementAt(charAt).ToString();
                    output[charAt] = varValues[varName];//print out
                    curval = varValues[varName];//set cur val
                    charAt++; 
                }      
            }
            return returnVal;
        }

        private List<bool> orOperation(List<bool> op1, List<bool> op2)
        {
            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++) 
            {
                //add the result of Or'ing the operands
                ResultList.Add(op1.ElementAt(i) || op2.ElementAt(i));
            }
            return ResultList;
        }


        private List<bool> andOperation(List<bool> op1, List<bool> op2)
        {
            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++)
            {
                //add the result of Or'ing the operands
                ResultList.Add(op1.ElementAt(i) && op2.ElementAt(i));
            }
            return ResultList;
        }


        private List<bool> ifthenOperation(List<bool> op1, List<bool> op2)
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


        private List<bool> notOperation(List<bool> op1)
        {
            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++)
            {
                //add the result of not'ing the operand
                ResultList.Add(!op1.ElementAt(i));
            }
            return ResultList;
        }


        private List<bool> BiConditionalOperation(List<bool> op1, List<bool> op2)
        {
            List<bool> ResultList = new List<bool>();
            for (int i = 0; i < op1.Count; i++)
            {
                //add the result of not'ing the operand
                ResultList.Add(op1.ElementAt(i) == op2.ElementAt(i));
            }

            return ResultList;
        }


        private Result SelectAndExecuteOperation(char op, List<bool> curval, Result res)
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
    }
}


