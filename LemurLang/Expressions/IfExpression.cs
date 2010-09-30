using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LemurLang.Interfaces;

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
            return string.Concat("if");
        }

        internal bool IsTrue
        {
            get
            {
                return true;
            }
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            //StringBuilder builder = new StringBuilder();
            
            //foreach (IExpression expression in this.Children)
            //{
            //    if(!this.IsTrue)
            //    {
            //        ElseIfExpression ifExpression = expression as ElseIfExpression;
            //        if(
                
            //    builder.Append(expression.DisplayTree(currentLevel + ((expression is ElseIfExpression) ? 0 : 1)));
            //}
            
            return string.Concat("if");
        }
    }
}
