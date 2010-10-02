using System;
using System.Collections.Generic;
using System.Text;
using LemurLang.Interfaces;
using System.IO;
using System.Text.RegularExpressions;

namespace LemurLang.Templates
{
    public abstract class BaseTemplate : ITemplate
    {
        public BaseTemplate(bool needsToBeEnded)
        {
            this.Children = new List<ITemplate>();
            this.NeedsToBeEnded = needsToBeEnded;
        }

        public List<ITemplate> Children { get; set; }

        public string UsedTag { get; set; }

        public ITemplate Parent { get; set; }
        
        public int IndexInTemplate { get; set; }

        public bool NeedsToBeEnded { get; set; }

        public override string ToString()
        {
            return this.UsedTag;
        }

        public string Arguments { get; set; }

        public virtual string DisplayTree(int currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('\t', currentLevel);
            builder.AppendLine(this.ToString());

            foreach (ITemplate expression in this.Children)
            {
                builder.AppendLine(expression.DisplayTree(currentLevel + 1));
            }

            return builder.ToString();
        }

        public virtual string Evaluate(EvaluationContext evaluationContext)
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
