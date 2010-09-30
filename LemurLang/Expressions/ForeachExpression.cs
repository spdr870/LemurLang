using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using LemurLang.Interfaces;
using System.Collections;
using LemurLang.Exceptions;
using System.Linq;

namespace LemurLang.Expressions
{
    public class ForeachExpression : BaseExpression
    {
        public ForeachExpression()
            : base(true)
        {
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            StringBuilder builder = new StringBuilder();

            string varToIntroduce = "";// this.Match.Groups["var"].Value;
            string sourceName = "";//this.Match.Groups["source"].Value;

            object source = evaluationContext.GetValue(sourceName);

            IEnumerable loopable = source as IEnumerable;
            if (loopable == null)
                throw new EvaluationException(sourceName + " is not able to be foreached");


            IExpression noDataExpression = null;
            IExpression beforeAllExpression = null;
            IExpression beforeExpression = null;
            IExpression oddExpression = null;
            IExpression evenExpression = null;
            IExpression afterExpression = null;
            IExpression betweenExpression = null;
            IExpression afterAllExpression = null;
            IExpression eachExpression = null;

            //TODO: All inner sections are optional, and they can appear in any order multiple times (sections with same name will have their content appended) 

            IExpression lastSubExpressionChild = null;
            foreach (IExpression child in this.Children.ToList())
            {
                ForeachSubExpression subExpression = child as ForeachSubExpression;
                if (subExpression != null)
                {
                    switch (subExpression.UsedTag)
                    {
                        case "beforeall":
                            beforeAllExpression = subExpression;
                            break;
                        case "before":
                            beforeExpression = subExpression;
                            break;
                        case "odd":
                            oddExpression = subExpression;
                            break;
                        case "even":
                            evenExpression = subExpression;
                            break;
                        case "each":
                            eachExpression = subExpression;
                            break;
                        case "after":
                            afterExpression = subExpression;
                            break;
                        case "between":
                            betweenExpression = subExpression;
                            break;
                        case "afterall":
                            afterAllExpression = subExpression;
                            break;
                        case "nodata":
                            noDataExpression = subExpression;
                            break;
                    }

                    lastSubExpressionChild = subExpression;
                }
                else
                {
                    if (lastSubExpressionChild != null)
                    {
                        lastSubExpressionChild.Children.Add(child);
                        this.Children.Remove(child);
                    }
                }
            }
            
            bool isEven = true;
            bool hadData = false;

            long index = 0;
            foreach (object item in loopable)
            {
                index++;
                hadData = true;
                isEven = !isEven;

                EvaluationContext subEvaluationContext = new EvaluationContext(
                    new Dictionary<string, object>() {
                        {varToIntroduce, item}
                    },
                    evaluationContext
                );

                if (beforeExpression != null)
                {
                    builder.Append(beforeExpression.Evaluate(subEvaluationContext));
                }

                if (!isEven && oddExpression != null)
                {
                    builder.Append(oddExpression.Evaluate(subEvaluationContext));
                }

                if (isEven && evenExpression != null)
                {
                    builder.Append(evenExpression.Evaluate(subEvaluationContext));
                }

                if (eachExpression != null)
                {
                    builder.Append(eachExpression.Evaluate(subEvaluationContext));
                }

                //#each (this is optional since its the default section)
                foreach (IExpression child in this.Children)
                {
                    if (!(child is ForeachSubExpression))
                    {
                        string childResult = child.Evaluate(subEvaluationContext);
                        builder.Append(childResult);
                    }
                }

                if (afterExpression != null)
                {
                    builder.Append(afterExpression.Evaluate(subEvaluationContext));
                }

                if (betweenExpression != null)
                {
                    builder.Append(betweenExpression.Evaluate(subEvaluationContext));
                }
            }

            if (hadData && beforeAllExpression != null)
            {
                builder.Insert(0, beforeAllExpression.Evaluate(evaluationContext));
            }

            if (hadData && afterAllExpression != null)
            {
                builder.Append(afterAllExpression.Evaluate(evaluationContext));
            }

            if (!hadData && noDataExpression != null)
            {
                builder.Append(noDataExpression.Evaluate(evaluationContext));
            }

            return builder.ToString();
        }
    }
}
