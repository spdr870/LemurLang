using System;
using System.Collections.Generic;
using System.Text;

namespace LemurLang.Exceptions
{
    public class UnhandledParseException : Exception
    {
        public UnhandledParseException(string message)
            : base(message)
        {
        }

        public UnhandledParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
