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
        public ExpressionHandler()
        {
            this.InitializeExpressions();
        }

        protected void InitializeExpressions()
        {

        }

        public IExpression BuildExpression(string template)
        {
            List<string> items = new List<string>(new string[]{
                "comment",
                "foreach",
                "if",
                "elseif",
                "else",
                "end"
            });
            
            RootExpression root = new RootExpression();
            IExpression currentNode = root;

            StringBuilder builder = new StringBuilder();

            for(int index = 0; index < template.Length; index++)
            {
                char c = template[index];

                if (c == '#')
                {
                    if (builder.Length > 0)
                    {
                        CreateAndAddTextExpression(currentNode, builder, index);
                    }
                    
                    StringBuilder hashbuildUp = new StringBuilder();

                    while (index + 1 < template.Length)
                    {
                        char nextChar = template[ index + 1];

                        bool inFirstRange = nextChar >= 'A' && nextChar <= 'Z';
                        bool inSecondRange = nextChar >= 'a' && nextChar <= 'z';
                        if (inFirstRange || inSecondRange)
                        {
                            index++;
                            hashbuildUp.Append(nextChar);
                        }
                        else
                        {
                            break;
                        }
                    }

                    string element = hashbuildUp.ToString();
                    if (hashbuildUp.Length > 0 && items.Contains(element))
                    {
                        if (element == "comment")
                        {
                            IExpression expression = new CommentExpression() { Parent = currentNode };
                            currentNode.Children.Add(expression);
                            currentNode = expression;
                        }
                        else if (element == "foreach")
                        {
                            IExpression expression = new ForeachExpression() { Parent = currentNode };
                            currentNode.Children.Add(expression);
                            currentNode = expression;
                        }
                        else if (element == "if")
                        {
                            IExpression expression = new IfExpression() { Parent = currentNode };
                            currentNode.Children.Add(expression);
                            currentNode = expression;
                        }
                        else if (element == "elseif")
                        {
                            IExpression expression = new ElseIfExpression() { Parent = currentNode };
                            currentNode.Children.Add(expression);

                        }
                        else if (element == "else")
                        {
                            IExpression expression = new ElseIfExpression() { Parent = currentNode };
                            currentNode.Children.Add(expression);

                        }
                        else if (element == "end")
                        {
                            currentNode = currentNode.Parent;
                        }
                    }
                    else
                    {
                        builder.Append(c);
                        builder.Append(hashbuildUp.ToString());
                    }
                    
                    hashbuildUp.Clear();
                }
                else if (c == '$')
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append(c);
                }
            }

            if (builder.Length > 0)
            {
                CreateAndAddTextExpression(currentNode, builder, 0);//todo
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

        private static void CreateAndAddTextExpression(IExpression currentNode, StringBuilder builder, int index)
        {
            TextExpression text = new TextExpression()
            {
                Text = builder.ToString(),
                Parent = currentNode,
                IndexInTemplate = index
            };
            currentNode.Children.Add(text);
            builder.Clear();
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
