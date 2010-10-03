﻿using System;
using System.Text;
using LemurLang.Exceptions;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class ElseIfTemplate : IfTemplate
    {
        public override string ToString()
        {
            return "ELSEIF: " + this.Arguments;
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            currentItem.Children.Add(this);

            if (nextChar != '(')
                throw new ParseException("Expected '(' after if");

            StringBuilder consumer = new StringBuilder();
            consumer.Append(nextChar);
            int stackCount = 1;
            index++;
            while (stackCount > 0 && index + 1 < template.Length)
            {
                nextChar = template[index + 1];
                if (nextChar == '(')
                    stackCount++;
                else if (nextChar == ')')
                    stackCount--;
                else if (stackCount > 0 && (nextChar == '\r' || nextChar == '\n'))
                    throw new ParseException("Expected ')' but encountered newline in if-statement");

                consumer.Append(nextChar);
                index++;
            }
            index++;

            if (stackCount > 0)
                throw new ParseException("More open parentheses than closing.");
            else if (template[index] == ')')
                throw new ParseException("More closing parentheses than opening.");

            this.Arguments = consumer.ToString();

            return new TemplateParseResult(currentItem, index);
        }

        public override void Evaluate(EvaluationContext evaluationContext, Action<string> write)
        {
            throw new InvalidOperationException("ElseIf cannot evaluate its children.");
        }
    }
}
