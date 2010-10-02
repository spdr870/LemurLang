using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang.Expression
{
    public enum TokenType
    {
        None,
        Number,
        Constant,
        Plus,
        Minus,
        Multiply,
        Divide,
        Exponent,
        UnaryMinus,
        LeftParenthesis,
        RightParenthesis
    }
}
