using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Expressions
{
    public class ElseIfExpression : IfExpression
    {
        public ElseIfExpression()
        {
            this.NeedsToBeEnded = false;
        }

        public override string ToString()
        {
            return "ELSEIF: " + this.State;
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            StringBuilder builder = new StringBuilder();

            foreach (IExpression expression in this.Children)
            {
                builder.Append(expression.Evaluate(evaluationContext));
            }

            return builder.ToString();
        }
    }
}
