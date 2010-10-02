using System;
using System.Text;
using System.Text.RegularExpressions;
using LemurLang.Exceptions;

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
            Regex contextProperty = new Regex(@"^\$\{([a-zA-Z0-9.]+)\}$");

            object left = null;
            if (Lhs != null)
            {
                Match lhsMatch = contextProperty.Match(Lhs);
                left = lhsMatch.Success ? contextGetter(lhsMatch.Groups[1].Value) : Lhs;
            }

            object right = null;
            if (Rhs != null)
            {
                Match rhsMatch = contextProperty.Match(Rhs);
                right = rhsMatch.Success ? contextGetter(rhsMatch.Groups[1].Value) : Rhs;
            }

            bool leftIsNull = left == null;
            bool rightIsNull = right == null;
            bool bothNull = leftIsNull && rightIsNull;
            bool anyNull = leftIsNull || rightIsNull;
            bool referenceEquals = object.ReferenceEquals(left, right);

            switch (Operator)
            {
                case "==":
                case "!=":
                    bool isEqualOperator = Operator == "==";

                    if (referenceEquals || bothNull)
                        return isEqualOperator;
                    else if (anyNull)
                        return !isEqualOperator;
                    else
                    {
                        Type leftType = left.GetType();
                        Type rightType = right.GetType();

                        object rightValueToCompare = (leftType==rightType) ? right : Convert.ChangeType(right, leftType);

                        bool doesEqual = left.Equals(rightValueToCompare);
                        if (isEqualOperator)
                            return doesEqual;
                        else
                            return !doesEqual;
                    }
                case "<=":
                case ">=":
                case "<":
                case ">":
                    if (anyNull)
                        throw new ConditionException("Cannot compare null values in condition: " + this.ToString());

                    decimal leftDecimal = ConvertToDecimal(left);
                    decimal rightDecimal = ConvertToDecimal(right);

                    switch (Operator)
                    {
                        case ">=":
                            return leftDecimal >= rightDecimal;
                        case "<=":
                            return leftDecimal <= rightDecimal;
                        case "<":
                            return leftDecimal < rightDecimal;
                        case ">":
                            return leftDecimal > rightDecimal;
                        default:
                            throw new ConditionException("Unknown operator: " + Operator);
                    }
                default:
                    throw new ConditionException("Unknown operator: " + Operator);
            }
        }

        private static decimal ConvertToDecimal(object value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (FormatException ex)
            {
                throw new ConditionException("Cannot convert value to numeric value: " + value.ToString(), ex);
            }
            catch (InvalidCastException ex)
            {
                throw new ConditionException("Cannot cast value to numeric value" + value.ToString(), ex);
            }
        }
    }
}
