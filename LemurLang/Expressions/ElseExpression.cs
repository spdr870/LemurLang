using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Interfaces;

namespace LemurLang.Expressions
{
    public class ElseExpression : ElseIfExpression
    {
        public override string ToString()
        {
            return "ELSE:";
        }
    }
}
