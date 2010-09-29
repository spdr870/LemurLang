using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LemurLang.Expressions
{
    public class TextExpression : BaseExpression
    {
        public TextExpression()
            : base(false)
        {
        }
        
        public string Text { get; set; }

        public override string ToString()
        {
            return string.Concat(
                "TEXT: ",
                Regex.Replace(this.Text.Substring(0, Math.Min(this.Text.Length, 20)), "\r\n|\r|\n", string.Empty),
                "..."
            );
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            return this.Text;
        }
    }
}
