using System;
using System.Collections.Generic;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public abstract class BaseTemplate : ITemplate
    {
        public BaseTemplate()
        {
            this.Children = new List<ITemplate>();
        }

        public List<ITemplate> Children { get; set; }

        public string UsedTag { get; set; }

        public ITemplate Parent { get; set; }
        
        public int IndexInTemplate { get; set; }

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

        public virtual void Evaluate(EvaluationContext evaluationContext, Action<string> write)
        {           
            foreach (ITemplate templateItem in this.Children)
            {
                templateItem.Evaluate(evaluationContext, write);
            }
        }

        public abstract TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar);
    }
}
