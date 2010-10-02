using System;
using System.Text;

namespace LemurLang.Conditions
{
    public class LogicalOperatorConditionElement : ConditionElement
    {
        public LogicalOperatorConditionElement(ConditionElement parent, string operatorText)
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
                builder.Append(" ");
                builder.Append(this.Operator);
                builder.Append(" ");
            }

            return builder.ToString();
        }

        public override bool Evaluate(Func<string, object> contextGetter)
        {
            throw new InvalidOperationException();
        }
    }
}
