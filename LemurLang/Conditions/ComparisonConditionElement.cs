using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang.Conditions
{
    public class ComparisonConditionElement : ConditionElement
    {
        public ComparisonConditionElement(ConditionElement parent, string lhs, string op, string rhs)
            : base(parent)
        {
            this.Lhs = lhs;
            this.Operator = op;
            this.Rhs = rhs;
        }

        public string Lhs { get; set; }
        public string Rhs { get; set; }
        public string Operator { get; set; }

        public override string ToString()
        {
            return string.Concat(Lhs, " ", Operator, " ", Rhs);
        }

        public override string DisplayTree(int currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(new string('\t', currentLevel));
            builder.Append(this.ToString());
            return builder.ToString();
        }

        public override bool Evaluate(Func<string, object> contextGetter)
        {
            object left = contextGetter(Lhs);
            object right = contextGetter(Rhs);

            bool leftIsNull = left == null;
            bool rightIsNull = right == null;
            bool bothNull = leftIsNull && rightIsNull;
            bool anyNull = leftIsNull || rightIsNull;

            switch (Operator)
            {
                case "==":
                    if (object.ReferenceEquals(left, right) || bothNull)
                        return true;
                    else if (anyNull)
                    {
                        return false;
                    }
                    else
                    {

                    }
                    return false;
                case "<=":
                    return false;
                case ">=":
                    return false;
                case "<":
                    return false;
                case ">":
                    return false;
                case "!=":
                    return false;
                default:
                    throw new Exception();
            }
        }
    }
}
