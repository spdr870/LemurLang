using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class PrintTemplate : BaseTemplate
    {
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

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            throw new InvalidOperationException();
        }
    }
}
