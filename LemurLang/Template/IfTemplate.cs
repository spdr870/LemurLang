using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Interfaces;
using LemurLang.Conditions;

namespace LemurLang.Templates
{
    public class IfTemplate : BaseTemplate
    {
        public IfTemplate()
            : base(true)
        {
        }

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

        protected virtual bool GetConditionEvaluation(EvaluationContext evaluationContext)
        {
            ConditionEngine conditionHandler = new ConditionEngine();
            ConditionElementList conditions = conditionHandler.Build(this.Arguments);

            bool result = conditions.Evaluate(evaluationContext.GetValue);
            return result;
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            bool result = GetConditionEvaluation(evaluationContext);

            StringBuilder builder = new StringBuilder();

            List<ITemplate> childrenToExecute = new List<ITemplate>();

            foreach (ITemplate expression in this.Children)
            {
                if (result && !(expression is ElseIfTemplate))
                {
                    builder.Append(expression.Evaluate(evaluationContext));
                }
                else if (result && expression is ElseIfTemplate)
                {
                    break;
                }
                else if (!result && expression is ElseIfTemplate)
                {
                    result = ((ElseIfTemplate)expression).GetConditionEvaluation(evaluationContext);
                }
            }

            return builder.ToString();
        }
    }
}
