using System;
using System.Collections.Generic;
using System.Text;

namespace LemurLang.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        {
        }

        public ParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
