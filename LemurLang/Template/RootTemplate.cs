using System;
using System.Collections.Generic;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class RootTemplate : BaseTemplate
    {
        public RootTemplate()
            : base(false)
        {
            this.UsedTag = "__ROOT";
        }

        public override string ToString()
        {
            return "ROOT";
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            throw new InvalidOperationException();
        }
    }
}
