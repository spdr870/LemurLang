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
            return "PRINT";
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            string reference = "";//this.Match.Groups["print"].Value;
            object evaluationResult = evaluationContext.GetValue(reference);

            return evaluationResult == null ? string.Empty : evaluationResult.ToString();
        }
    }
}
