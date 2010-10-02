using System;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class RootTemplate : BaseTemplate
    {
        public RootTemplate()
        {
            this.UsedTag = "__ROOT";
        }

        public override string ToString()
        {
            return "ROOT";
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            throw new InvalidOperationException();
        }

        public string Evaluate(EvaluationContext evaluationContext)
        {
            StringBuilder builder = new StringBuilder();

            Action<string> writer = (x) => builder.Append(x);

            this.Evaluate(evaluationContext, writer);

            return builder.ToString();
        }
    }
}
