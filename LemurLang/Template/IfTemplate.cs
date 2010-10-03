using System;
using System.Text;
using LemurLang.Conditions;
using LemurLang.Exceptions;
using LemurLang.Interfaces;
using LemurLang.Expression;

namespace LemurLang.Templates
{
    public class IfTemplate : BaseTemplate
    {
        public override string DisplayTree(int currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('\t', currentLevel);
            builder.AppendLine(this.ToString());

            foreach (ITemplate expression in this.Children)
            {
                builder.AppendLine(expression.DisplayTree(currentLevel + ((expression is ElseIfTemplate) ? 0 : 1)));
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            return "IF: " + this.Arguments;
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
            while (stackCount > 0)
            {
                if (index + 1 >= template.Length)
                    throw new ParseException("Unexpected end of template");
                
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

            if (index >= template.Length)
                throw new ParseException("Unexpected end of template");

            if (stackCount > 0)
                throw new ParseException("More open parentheses than closing.");
            else if (template[index] == ')')
                throw new ParseException("More closing parentheses than opening.");

            this.Arguments = consumer.ToString();

            return new TemplateParseResult(this, index);
        }

        protected virtual bool GetConditionEvaluation(EvaluationContext evaluationContext)
        {
            ReversePolishNotation rpn = new ReversePolishNotation(this.Arguments);
            object value = rpn.Evaluate(evaluationContext.GetValue);

            return Convert.ToBoolean(value);
        }

        public override void Evaluate(EvaluationContext evaluationContext, Action<string> write)
        {
            //get result of if statement
            bool result = GetConditionEvaluation(evaluationContext);

            //walk through children (could be TEXT, PRINT, IF, ELSEIF, ELSE, etc)
            foreach (ITemplate templateItem in this.Children)
            {
                if (result && !(templateItem is ElseIfTemplate)) //if the current condition was valid
                {
                    templateItem.Evaluate(evaluationContext, write); //evaluate child
                }
                else if (result && templateItem is ElseIfTemplate) //if the current condition was valid, but the current child is ELSE(IF)
                {
                    break;//stop executing children, all work is done
                }
                else if (!result && templateItem is ElseIfTemplate) //the condition wasn't valid, but ELSE(IF) was found and that one might have a valid condition
                {
                    //get result of else(if) statement. Else is always true
                    result = ((ElseIfTemplate)templateItem).GetConditionEvaluation(evaluationContext);
                }
            }
        }
    }
}
