using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicResolver
{
    public class ResultPrinter
    {
        private int getNumOfUniqueLettersInString(string toResolve)
        {
            List<char> alreadyCounted = new List<char>();
            foreach (char c in toResolve)
            {
                if (StringUtilities.IsEnglishLetter(c))
                {
                    if (!alreadyCounted.Contains(c))
                    {
                        alreadyCounted.Add(c);
                    }
                }
            }
            return alreadyCounted.Count;
        }


        public void PrintOutput(string toResolve, List<bool>[] output)
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
                    if (output[i] == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {

                        if (output[i].Count == 0)
                        {
                            Console.Write(" ");
                        }
                        else
                        {
                            if (output[i].ElementAt(j) == true)
                            {
                                Console.Write("T");
                            }
                            else
                            {
                                Console.Write("F");
                            }
                        }
                    }
                    Console.Write(" ");

                }//end of inner for
                Console.WriteLine();
            }//end of outer for

        }
    }
}
