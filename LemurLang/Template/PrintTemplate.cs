using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang.Templates
{
    public class PrintTemplate : BaseTemplate
    {
        public PrintTemplate()
            : base(false)
        {

        }

        public override string ToString()
        {
            return "PRINT: " + this.Arguments;
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            string reference = this.Arguments;
            object evaluationResult = evaluationContext.GetValue(reference);

            return evaluationResult == null ? string.Empty : evaluationResult.ToString();
        }
    }
}
