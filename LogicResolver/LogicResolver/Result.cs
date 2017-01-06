using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicResolver
{
    public class Result
    {
        public Result(){}

        public Result(string toResolve, List<bool> curval, int charAt)
        {
            this.toResolve = toResolve;
            this.curval = curval;
            this.charAt = charAt;
        }
       public string toResolve { get; set; }

       public List<bool> curval {get; set;}
       
       public int charAt { get; set; }
       
    }
}
