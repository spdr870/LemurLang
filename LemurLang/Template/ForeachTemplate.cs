using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LemurLang.Exceptions;
using LemurLang.Interfaces;

namespace LemurLang.Templates
{
    public class ForeachTemplate : BaseTemplate
    {
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
                if (index + 1 >= template.Length)
                    throw new ParseException("Unexpected end of template");
                
                nextChar = template[index + 1];
                if (nextChar == '\r' || nextChar == '\n')
                    throw new ParseException("Expected ')' but encountered newline after foreach");

                consumer.Append(nextChar);
                index++;
            }
            index++;

            if (index >= template.Length)
                throw new ParseException("Unexpected end of template");

            consumer.RemoveLastCharacter();
            
            this.Arguments = consumer.ToString();

            return new TemplateParseResult(this, index);
        }

        public override void Evaluate(EvaluationContext evaluationContext, Action<string> write)
        {
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
            bool isFirst = true;

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

                if (isFirst && beforeAllTemplate != null)
                {
                    beforeAllTemplate.Evaluate(evaluationContext, write);
                }

                if (beforeTemplate != null)
                {
                    beforeTemplate.Evaluate(subEvaluationContext, write);
                }

                if (!isEven && oddTemplate != null)
                {
                    oddTemplate.Evaluate(subEvaluationContext, write);
                }

                if (isEven && evenTemplate != null)
                {
                    evenTemplate.Evaluate(subEvaluationContext, write);
                }

                if (eachTemplate != null)
                {
                    eachTemplate.Evaluate(subEvaluationContext, write);
                }

                //#each (this is optional since its the default section)
                foreach (ITemplate child in this.Children)
                {
                    if (!(child is ForeachSubTemplate))
                    {
                        child.Evaluate(subEvaluationContext, write);
                    }
                }

                if (afterTemplate != null)
                {
                    afterTemplate.Evaluate(subEvaluationContext, write);
                }

                if (betweenTemplate != null)
                {
                    betweenTemplate.Evaluate(subEvaluationContext, write);
                }

                isFirst = false;
            }

            if (hadData && afterAllTemplate != null)
            {
                afterAllTemplate.Evaluate(evaluationContext, write);
            }

            if (!hadData && noDataTemplate != null)
            {
                noDataTemplate.Evaluate(evaluationContext, write);
            }
        }
    }
}
