using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemurLang
{
    public static class StringBuilderExtensions
    {
        public static void Clear(this StringBuilder builder)
        {
            builder.Remove(0, builder.Length);
        }

        public static void Prepend(this StringBuilder builder, string value)
        {
            builder.Insert(0, value);
        }
        public static void Prepend(this StringBuilder builder, char value)
        {
            builder.Insert(0, value);
        }

        public static void RemoveLastCharacter(this StringBuilder builder)
        {
            builder.Remove(builder.Length - 1, 1);
        }
    }
}
