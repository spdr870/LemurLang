using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class ForeachSubTemplate : BaseTemplate
    {
        public ForeachSubTemplate()
            : base(false)
        {
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            currentItem.Children.Add(this);

            return new TemplateParseResult(currentItem, index);
        }
    }
}
