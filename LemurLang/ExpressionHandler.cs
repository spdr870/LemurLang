using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LemurLang.Expressions;
using LemurLang.Interfaces;
using LemurLang.Exceptions;
using System.Linq;

namespace LemurLang
{
    public class ExpressionHandler
    {
        private Dictionary<string, Type> _expressionsbyName;
        private List<string> _expressionRegularExpressions;

        public ExpressionHandler()
        {
            this.InitializeExpressions();
        }

        public void RegisterExpression<T>(string name, string regex) where T : IExpression, new()
        {
            _expressionsbyName[name] = typeof(T);
            _expressionRegularExpressions.Add(regex);
        }

        protected void InitializeExpressions()
        {
            _expressionsbyName = new Dictionary<string, Type>();
            _expressionRegularExpressions = new List<string>();

            //#foreach(${person} in ${people})
            this.RegisterExpression<ForeachExpression>("foreach", @"(#(?'tag'foreach)\((?:\s*)\$\{(?'var'[a-zA-Z0-9.]+)\}(?:\s+)in(?:\s+)\$\{(?'source'[a-zA-Z0-9.]+)\}(?:\s*)\))");
            
            this.RegisterExpression<ForeachSubExpression>("beforeall", @"(#(?'tag'beforeall))");
            this.RegisterExpression<ForeachSubExpression>("before", @"(#(?'tag'before))");
            this.RegisterExpression<ForeachSubExpression>("odd", @"(#(?'tag'odd))");
            this.RegisterExpression<ForeachSubExpression>("even", @"(#(?'tag'even))");
            this.RegisterExpression<ForeachSubExpression>("each", @"(#(?'tag'each))");
            this.RegisterExpression<ForeachSubExpression>("afterall", @"(#(?'tag'afterall))");
            this.RegisterExpression<ForeachSubExpression>("after", @"(#(?'tag'after))");
            this.RegisterExpression<ForeachSubExpression>("between", @"(#(?'tag'between))");
            this.RegisterExpression<ForeachSubExpression>("nodata", @"(#(?'tag'nodata))");
            
            this.RegisterExpression<CommentExpression>("comment", @"(#(?'tag'comment))");
            
            this.RegisterExpression<PrintExpression>("$", @"((?'tag'\$)\{(?'print'[a-zA-Z0-9.]+)\})");

            this.RegisterExpression<ElseIfExpression>("elseif", @"(#(?'tag'elseif)(?'condition'(\(.+)))");
            this.RegisterExpression<IfExpression>("if", @"(#(?'tag'if)(?'condition'(\(.+)))");
        }

        public IExpression BuildExpression(string template)
        {
            RootExpression root = new RootExpression();
            IExpression currentNode = root;

            string regex = string.Concat(
                string.Join(
                    "|",
                    _expressionRegularExpressions
                        //.Select(x => string.Concat(@"(([\t\r\n]*)" + x, ")"))
                        .ToArray()
                ),
                "|(#(?'tag'end))"
            );

            Match match = Regex.Match(template, regex);

            int lastTextPosition = 0;

            while (match.Success)
            {
                string currentText = template.Substring(lastTextPosition, match.Index - lastTextPosition);

                if (!string.IsNullOrEmpty(currentText))
                {
                    TextExpression textExpression = new TextExpression();
                    textExpression.Text = currentText;
                    textExpression.UsedTag = null;
                    textExpression.Match = match;
                    textExpression.Parent = currentNode;
                    textExpression.IndexInTemplate = match.Index;
                    currentNode.Children.Add(textExpression);
                }

                bool endTagFound = match.Value == "#end";
                string expressionTag = match.Groups["tag"].Value;

                // non-end expression tag found
                if (!endTagFound)
                {
                    Type typeToFind = null;
                    if (_expressionsbyName.TryGetValue(expressionTag, out typeToFind)) // if expression exists
                    {
                        IExpression builtExpression = Activator.CreateInstance(typeToFind) as IExpression;
                        builtExpression.UsedTag = expressionTag;
                        builtExpression.Match = match;
                        builtExpression.Parent = currentNode;
                        builtExpression.IndexInTemplate = match.Index;

                        currentNode.Children.Add(builtExpression);
                        currentNode = builtExpression;

                        if (!builtExpression.NeedsToBeEnded)
                        {
                            currentNode = currentNode.Parent;
                        }
                    }
                    else
                    {
                        TextExpression textExpression = new TextExpression();
                        textExpression.Text = match.Value;
                        textExpression.UsedTag = null;
                        textExpression.Match = match;
                        textExpression.Parent = currentNode;
                        textExpression.IndexInTemplate = match.Index;
                        currentNode.Children.Add(textExpression);
                    }
                }
                else // end tag
                {
                    if (!currentNode.NeedsToBeEnded || currentNode.Parent == null)
                    {
                        int lineNumber1 = GetLineNumberFromIndex(template, currentNode.IndexInTemplate);
                        int lineNumber2 = GetLineNumberFromIndex(template, match.Index);
                        throw new ParseException(
                            string.Format(
                                "Cannot use end tag here. Hint -->\r\nLine numbers: {0}, {1}\r\nCurrent Node: {2}",
                                lineNumber1,
                                lineNumber2,
                                currentNode.UsedTag
                            )
                        );
                    }
                    else
                    {
                        currentNode = currentNode.Parent;
                    }
                }

                lastTextPosition = match.Index + match.Length;
                match = match.NextMatch();
            }

            string endText = template.Substring(lastTextPosition);
            if (!string.IsNullOrEmpty(endText))
            {
                TextExpression textExpr = new TextExpression();
                textExpr.Text = endText;
                currentNode.Children.Add(textExpr);
            }

            if (currentNode != root)
            {
                int lineNumber = GetLineNumberFromIndex(template, currentNode.IndexInTemplate);
                throw new ParseException(
                    string.Format("Tag '{0}' needs to be ended. Hint -->\r\nLine number: {1}",
                        currentNode.UsedTag,
                        lineNumber
                    )
                );
            }

            return root;
        }

        internal int GetLineNumberFromIndex(string content, int index)
        {
            if (index >= content.Length)
                return -1;
            else
                return Regex.Matches(content.Substring(0, index), @"(\r\n|\r|\n)").Count + 1;
        }
    }
}
