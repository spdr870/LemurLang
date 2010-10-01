using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang.Conditions
{
    public class ConditionElementList : ConditionElement
    {
        public IList<ConditionElement> Children { get; set; }

        public ConditionElementList(ConditionElement parent)
            : base(parent)
        {
            this.Children = new List<ConditionElement>();
        }

        public override string ToString()
        {
            return "[]";
        }

        public string DisplayTree()
        {
            return DisplayTree(-1);
        }

        public override string DisplayTree(int currentLevel)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(new string('\t', currentLevel + 1));
            builder.AppendLine("(");
            foreach (ConditionElement element in this.Children)
            {
                builder.AppendLine(element.DisplayTree(currentLevel + 1));
            }
            builder.Append(new string('\t', currentLevel + 1));
            builder.AppendLine(")");

            return builder.ToString();
        }

        public override bool Evaluate(Func<string, object> contextGetter)
        {
            bool result = false;

            bool first = true;
            LogicalOperatorConditionElement lastLogical = null;
            foreach (ConditionElement element in this.Children)
            {
                if (first)
                {
                    result = element.Evaluate(contextGetter);
                    first = false;
                }
                else
                {
                    LogicalOperatorConditionElement logical = element as LogicalOperatorConditionElement;
                    if (logical == null)
                    {
                        if (lastLogical.Operator == "&&")
                        {
                            if (!result)
                                return false; //short circuit

                            bool currentResult = element.Evaluate(contextGetter);
                            result = result && currentResult;
                        }
                        else if (lastLogical.Operator == "||")
                        {
                            if (result)
                                return true; //short circuit
                            
                            bool currentResult = element.Evaluate(contextGetter);
                            result = result || currentResult;
                        }
                    }
                    else
                    {
                        lastLogical = logical;
                    }
                }
            }

            return result;
        }
    }
}
