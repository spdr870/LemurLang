using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using LemurLang.Interfaces;
using System.Collections;
using LemurLang.Exceptions;
using System.Linq;

namespace LemurLang.Templates
{
    public class ForeachTemplate : BaseTemplate
    {
        public ForeachTemplate()
            : base(true)
        {
        }

        public override string ToString()
        {
            return "FOREACH: " + this.Arguments;
        }

        public override TemplateParseResult Parse(string template, ITemplate currentItem, int index, char nextChar)
        {
            currentItem.Children.Add(this);

            if (nextChar != '(')
                throw new ParseException("Expected '(' after foreach");

            StringBuilder consumer = new StringBuilder();
            index++;
            while (nextChar != ')')
            {
                nextChar = template[index + 1];
                if (nextChar == '\r' || nextChar == '\n')
                    throw new ParseException("Expected ')' but encountered newline after foreach");

                consumer.Append(nextChar);
                index++;
            }
            index++;

            consumer.RemoveLastCharacter();
            
            this.Arguments = consumer.ToString();

            return new TemplateParseResult(this, index);
        }

        public override string Evaluate(EvaluationContext evaluationContext)
        {
            StringBuilder builder = new StringBuilder();

            Match match = Regex.Match(this.Arguments, @"\$\{(?'var'[a-zA-Z0-9.]+)\}(?:\s+)in(?:\s+)\$\{(?'source'[a-zA-Z0-9.]+)\}");
            if (!match.Success)
                throw new EvaluationException("Could not interpret foreach: " + this.Arguments);

            string varToIntroduce = match.Groups["var"].Value;
            string sourceName = match.Groups["source"].Value;

            object source = evaluationContext.GetValue(sourceName);

            IEnumerable loopable = source as IEnumerable;
            if (loopable == null)
                throw new EvaluationException(sourceName + " is not able to be foreached");


            ITemplate noDataTemplate = null;
            ITemplate beforeAllTemplate = null;
            ITemplate beforeTemplate = null;
            ITemplate oddTemplate = null;
            ITemplate evenTemplate = null;
            ITemplate afterTemplate = null;
            ITemplate betweenTemplate = null;
            ITemplate afterAllTemplate = null;
            ITemplate eachTemplate = null;

            //TODO?: All inner sections are optional, and they can appear in any order multiple times (sections with same name will have their content appended) 

            ITemplate lastSubTemplateChild = null;
            foreach (ITemplate child in this.Children.ToList())
            {
                ForeachSubTemplate subTemplate = child as ForeachSubTemplate;
                if (subTemplate != null)
                {
                    switch (subTemplate.UsedTag)
                    {
                        case "beforeall":
                            beforeAllTemplate = subTemplate;
                            break;
                        case "before":
                            beforeTemplate = subTemplate;
                            break;
                        case "odd":
                            oddTemplate = subTemplate;
                            break;
                        case "even":
                            evenTemplate = subTemplate;
                            break;
                        case "each":
                            eachTemplate = subTemplate;
                            break;
                        case "after":
                            afterTemplate = subTemplate;
                            break;
                        case "between":
                            betweenTemplate = subTemplate;
                            break;
                        case "afterall":
                            afterAllTemplate = subTemplate;
                            break;
                        case "nodata":
                            noDataTemplate = subTemplate;
                            break;
                    }

                    lastSubTemplateChild = subTemplate;
                }
                else
                {
                    if (lastSubTemplateChild != null)
                    {
                        lastSubTemplateChild.Children.Add(child);
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

                if (beforeTemplate != null)
                {
                    builder.Append(beforeTemplate.Evaluate(subEvaluationContext));
                }

                if (!isEven && oddTemplate != null)
                {
                    builder.Append(oddTemplate.Evaluate(subEvaluationContext));
                }

                if (isEven && evenTemplate != null)
                {
                    builder.Append(evenTemplate.Evaluate(subEvaluationContext));
                }

                if (eachTemplate != null)
                {
                    builder.Append(eachTemplate.Evaluate(subEvaluationContext));
                }

                //#each (this is optional since its the default section)
                foreach (ITemplate child in this.Children)
                {
                    if (!(child is ForeachSubTemplate))
                    {
                        string childResult = child.Evaluate(subEvaluationContext);
                        builder.Append(childResult);
                    }
                }

                if (afterTemplate != null)
                {
                    builder.Append(afterTemplate.Evaluate(subEvaluationContext));
                }

                if (betweenTemplate != null)
                {
                    builder.Append(betweenTemplate.Evaluate(subEvaluationContext));
                }
            }

            if (hadData && beforeAllTemplate != null)
            {
                builder.Insert(0, beforeAllTemplate.Evaluate(evaluationContext));
            }

            if (hadData && afterAllTemplate != null)
            {
                builder.Append(afterAllTemplate.Evaluate(evaluationContext));
            }

            if (!hadData && noDataTemplate != null)
            {
                builder.Append(noDataTemplate.Evaluate(evaluationContext));
            }

            return builder.ToString();
        }
    }
}
