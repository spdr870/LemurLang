using System;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class PrintTemplate : BaseTemplate
    {
        public override string ToString()
        {
            return "PRINT: " + this.Arguments;
        }

        public override void Evaluate(EvaluationContext evaluationContext, Action<string> write)
        {
            string reference = this.Arguments;
            object evaluationResult = evaluationContext.GetValue(reference);

            write( evaluationResult == null ? string.Empty : evaluationResult.ToString() );
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            throw new InvalidOperationException();
        }
    }
}
