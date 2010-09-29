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
    }
}
