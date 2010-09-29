using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang.Conditions
{
    public class StringConditionElement : ConditionElement
    {
        public StringConditionElement(ConditionElement parent, string text)
            : base(parent)
        {
            this.Text = text;
        }

        public string Text { get; set; }

        public override string ToString()
        {
            return this.Text;
        }

        public override string DisplayTree(int currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            if (this.Text != null)
            {
                builder.Append(new string('\t', currentLevel));
                builder.Append(this.Text);
            }
            return builder.ToString();
        }
    }
}
