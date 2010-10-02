using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            return this.Text;
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            throw new InvalidOperationException();
        }
    }
}
