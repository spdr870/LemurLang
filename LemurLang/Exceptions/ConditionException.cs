using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang.Exceptions
{
    public class ConditionException : Exception
    {
        public ConditionException(string message) : base(message)
        {
        }

        public ConditionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
