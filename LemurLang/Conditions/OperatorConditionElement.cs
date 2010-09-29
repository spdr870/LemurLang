using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang.Conditions
{
    public class OperatorConditionElement : ConditionElement
    {
        public OperatorConditionElement(ConditionElement parent, string operatorText)
            : base(parent)
        {
            this.Operator = operatorText;
        }

        public string Operator { get; set; }

        public override string ToString()
        {
            return this.Operator;
        }

        public override string DisplayTree(int currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            if (this.Operator != null)
            {
                builder.Append(new string('\t', currentLevel));
                builder.Append(this.Operator);
            }

            return builder.ToString();
        }
    }
}
