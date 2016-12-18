using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicResolver
{
    class Program
    {
        static void Main(string[] args)
        {
            PropositionalLogicResolver p = new PropositionalLogicResolver();
            while (true)
            {
                Console.WriteLine("Please input propositional logic statement to be evaluated: ");

                string userinput = Console.ReadLine();
                userinput = userinput.Replace(" ", string.Empty);

                string err = p.ProperSyntax(userinput);
                string paranCheck = p.CheckBalancedParans(userinput);

                if ( err == "true" & paranCheck == "true")
                {
                    p.Resolve(userinput);
                }
                else
                {
                    if(!err.Equals("true"))
                    {
                        Console.WriteLine(err);
                        Console.WriteLine(userinput);
                        int result = Int32.Parse(Regex.Match(err, @"\d+").Value);
                        for (int i = 0; i <= result; i++) {
                            if (i < result) { Console.Write(" "); } else { Console.Write("^"); };
                        }
                        Console.WriteLine();
                    }
                    if (!paranCheck.Equals("true"))
                    {
                        Console.WriteLine(paranCheck);
                        Console.WriteLine(userinput);
                        Console.WriteLine();
                    }
                   
                }

                
            }
        }
    }
}
