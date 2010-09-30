using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang
{
    public static class StringBuildExtensions
    {
        public static void Clear(this StringBuilder builder)
        {
            builder.Remove(0, builder.Length);
        }

        private static void RemoveLastCharacter(this StringBuilder builder)
        {
            builder.Remove(builder.Length - 1, builder.Length);
        }
    }
}
