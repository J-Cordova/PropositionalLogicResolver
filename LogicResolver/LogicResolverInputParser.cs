using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicResolver
{
    public class LogicResolverInputParser
    {
        public Dictionary<string, List<bool>> GetVariableTruthValues(String expression)
        {
            Dictionary<string, List<bool>> values = new Dictionary<string, List<bool>>();
            List<string> vars = new List<string>();

            foreach (char s in expression)
            {

                if (StringUtilities.IsEnglishLetter(s) & !vars.Contains(s.ToString()))
                {
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
                    //This mimics the behavior of writing truth tables
                    //It start the first column of half true half false ie T T F F
                    //Then it halves that so instead of 2 T and 2 F now its 1 T and 1 F ie T F T F
                    //If we had 3 variables it would be 4T and 4F, then 2T 2F 2T 2F, then 1T 1F 1T 1F 1T 1F 1T, 1F

                    //T T for 2
                    //T F
                    //F T
                    //F F

                    // T T T 
                    // T T F
                    // T F T
                    // T F F
                    // F T T
                    // F T F
                    // F F T
                    // F F F
                    if ((j % (numTF / Math.Pow(2, i + 1))) == 0 & j != 0)
                    {
                        toWrite = !toWrite;
                    }
                    tvalues.Add(toWrite);

                }//end for

                //put values for variable in dictionary
                if (!values.ContainsKey(vars.ElementAt(i)))
                    values.Add(vars.ElementAt(i), tvalues);
            }
            return values;
        }
    }
}
