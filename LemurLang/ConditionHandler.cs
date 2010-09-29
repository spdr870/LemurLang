using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang
{
    public class Condition
    {
        public IList<Condition> Subconditions { get; private set; }
        
        public Condition()
        {
            this.Subconditions = new List<Condition>();
        }
    }
    
    public class ConditionHandler
    {
        public Condition Build(string input)
        {
            Condition rootCondition = new Condition();
            
            foreach (char c in input)
            {
            }

            return rootCondition;
        }
    }
}
