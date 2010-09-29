using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang.Expressions
{
    public class PrintExpression : BaseExpression
    {
        public PrintExpression()
            : base(false)
        {

        }

        public override string ToString()
        {
            return "PRINT: " + this.Match.Groups["print"].Value;
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            return evaluationContext.GetValue(this.Match.Groups["print"].Value).ToString();
        }
    }
}
