using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LemurLang.Templates
{
    public class TextTemplate : BaseTemplate
    {
        public TextTemplate()
            : base(false)
        {
        }
        
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
    }
}
