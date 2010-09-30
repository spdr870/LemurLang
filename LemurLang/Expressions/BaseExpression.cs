using System;
using System.Collections.Generic;
using System.Text;
using LemurLang.Interfaces;
using System.IO;
using System.Text.RegularExpressions;

namespace LemurLang.Expressions
{
    public abstract class BaseExpression : IExpression
    {
        protected ExpressionHandler _expressionHandler;

        public BaseExpression(bool needsToBeEnded)
        {
            this.Children = new List<IExpression>();
            this.NeedsToBeEnded = needsToBeEnded;
        }

        public List<IExpression> Children { get; set; }

        public string UsedTag { get; set; }

        public IExpression Parent { get; set; }
        
        public int IndexInTemplate { get; set; }

        public bool NeedsToBeEnded { get; set; }

        public override string ToString()
        {
            return this.UsedTag;
        }

        public virtual string DisplayTree(int currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('\t', currentLevel);
            builder.AppendLine(this.ToString());

            foreach (IExpression expression in this.Children)
            {
                builder.AppendLine(expression.DisplayTree(currentLevel + 1));
            }

            return builder.ToString();
        }

        public virtual string Evaluate(EvaluationContext evaluationContext)
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
