using System;
using System.Text;
using LemurLang.Conditions;
using LemurLang.Exceptions;
using LemurLang.Interfaces;

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

            this.Arguments = consumer.ToString();

            return new TemplateParseResult(this, index);
        }

        protected virtual bool GetConditionEvaluation(EvaluationContext evaluationContext)
        {
            ConditionEngine conditionHandler = new ConditionEngine();
            ConditionElementList conditions = conditionHandler.Build(this.Arguments);

            bool result = conditions.Evaluate(evaluationContext.GetValue);
            return result;
        }

        public override void Evaluate(EvaluationContext evaluationContext, Action<string> write)
        {
            bool result = GetConditionEvaluation(evaluationContext);

            foreach (ITemplate templateItem in this.Children)
            {
                if (result && !(templateItem is ElseIfTemplate))
                {
                    templateItem.Evaluate(evaluationContext, write);
                }
                else if (result && templateItem is ElseIfTemplate)
                {
                    break;
                }
                else if (!result && templateItem is ElseIfTemplate)
                {
                    result = ((ElseIfTemplate)templateItem).GetConditionEvaluation(evaluationContext);
                }
            }
        }
    }
}
