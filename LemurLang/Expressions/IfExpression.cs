using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Interfaces;
using LemurLang.Conditions;

namespace LemurLang.Expressions
{
    public class IfExpression : BaseExpression
    {
        public IfExpression()
            : base(true)
        {
        }

        public override string DisplayTree(int currentLevel)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('\t', currentLevel);
            builder.AppendLine(this.ToString());

            foreach (IExpression expression in this.Children)
            {
                builder.AppendLine(expression.DisplayTree(currentLevel + ((expression is ElseIfExpression) ? 0 : 1)));
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            return "IF: " + this.State;
        }

        internal bool GetConditionEvaluation(EvaluationContext evaluationContext)
        {
            ConditionEngine conditionHandler = new ConditionEngine();
            ConditionElementList conditions = conditionHandler.Build(this.State);

            bool result = conditions.Evaluate(evaluationContext.GetValue);
            return result;
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            bool result = GetConditionEvaluation(evaluationContext);

            StringBuilder builder = new StringBuilder();

            List<IExpression> childrenToExecute = new List<IExpression>();

            foreach (IExpression expression in this.Children)
            {
                if (result && !(expression is ElseIfExpression))
                {
                    builder.Append(expression.Evaluate(evaluationContext));
                }
                else if (result && expression is ElseIfExpression)
                {
                    break;
                }
                else if (!result && expression is ElseIfExpression)
                {
                    result = ((ElseIfExpression)expression).GetConditionEvaluation(evaluationContext);
                }
            }

            return builder.ToString();
        }
    }
}
