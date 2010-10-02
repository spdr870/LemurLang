using System;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class TextTemplate : BaseTemplate
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return "TEXT";
            //return string.Concat(
            //    "TEXT: ",
            //    this.Text
            //);
        }

        public override void Evaluate(EvaluationContext evaluationContext, Action<string> write)
        {
            write(this.Text);
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            throw new InvalidOperationException();
        }
    }
}
