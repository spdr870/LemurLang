using System;
using System.Collections.Generic;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Expressions
{
    public class CommentExpression : BaseExpression
    {
        public CommentExpression()
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
