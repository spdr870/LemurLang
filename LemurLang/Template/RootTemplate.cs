using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
