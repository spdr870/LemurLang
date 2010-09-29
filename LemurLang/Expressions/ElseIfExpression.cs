using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return string.Concat("elseif: ", this.Match.Groups["condition"].Value);
        }
    }
}
