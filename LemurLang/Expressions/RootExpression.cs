using System;
using System.Collections.Generic;
using System.Text;

namespace LemurLang.Expressions
{
    public class RootExpression : BaseExpression
    {
        public RootExpression()
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
