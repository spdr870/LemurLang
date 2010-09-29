using System;
using System.Collections.Generic;
using System.Text;

namespace LemurLang.Exceptions
{
    public class EvaluationException : Exception
    {
        public EvaluationException(string message)
            : base(message)
        {
        }
    }
}
