using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicResolver
{
    public class LogicResolverRunner
    {
        private PropositionalLogicResolver resolver = new PropositionalLogicResolver();
        private LogicResolverInputValidator validator = new LogicResolverInputValidator();

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Please input propositional logic statement to be evaluated: ");

                string userinput = Console.ReadLine();
                userinput = userinput.Replace(" ", string.Empty);

                string err = validator.CheckSyntax(userinput);
                string paranCheck = validator.CheckBalancedParans(userinput);

                if (err == "true" & paranCheck == "true")
                {
                    resolver.Resolve(userinput);
                }
                else
                {
                    if (!err.Equals("true"))
                    {
                        Console.WriteLine(err);
                        Console.WriteLine(userinput);
                        int result = Int32.Parse(Regex.Match(err, @"\d+").Value);
                        for (int i = 0; i <= result; i++)
                        {
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
