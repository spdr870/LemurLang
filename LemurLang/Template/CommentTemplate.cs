using System;
using System.Collections.Generic;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class CommentTemplate : BaseTemplate
    {
        public CommentTemplate()
            : base(true)
        {
        }

        public override string ToString()
        {
            return "COMMENT";
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            return string.Empty;
        }
    }
}
