using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class ElseIfTemplate : IfTemplate
    {
        public ElseIfTemplate()
        {
            this.NeedsToBeEnded = false;
        }

        public override string ToString()
        {
            return "ELSEIF: " + this.Arguments;
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            StringBuilder builder = new StringBuilder();

            foreach (ITemplate expression in this.Children)
            {
                builder.Append(expression.Evaluate(evaluationContext));
            }

            return builder.ToString();
        }
    }
}
