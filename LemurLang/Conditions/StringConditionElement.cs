using System;
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

        public override bool Evaluate(Func<string, object> contextGetter)
        {
            if (this.Text.Equals("true", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (this.Text.Equals("false", StringComparison.OrdinalIgnoreCase))
                return false;

            return Convert.ToBoolean(contextGetter(this.Text));
        }
    }
}
