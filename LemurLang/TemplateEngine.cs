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
    public class TemplateEngine
    {
        public IExpression BuildExpression(string template)
        {
            List<string> foreachSubItems = new List<string>(new string[]{
                "beforeall", "before", "odd", "even", "each", "after", "between", "afterall", "nodata"
            });
            
            List<string> items = new List<string>(new string[]{
                "comment",
                "foreach",
                "if",
                "elseif",
                "else",
                "end"
            }.Concat(foreachSubItems));
            
            RootExpression root = new RootExpression();
            IExpression currentNode = root;

            StringBuilder builder = new StringBuilder();

            for(int index = 0; index < template.Length; index++)
            {
                char c = template[index];

                if (c == '#')
                {
                    StringBuilder hashbuildUp = new StringBuilder();
                    char? nextChar = null;

                    while (index + 1 < template.Length)
                    {
                        nextChar = template[ index + 1];

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

                    bool elementHandled = true;
                    if (hashbuildUp.Length > 0 && items.Contains(element))
                    {
                        if (builder.Length > 0)
                        {
                            CreateAndAddTextExpression(currentNode, builder, index);
                        }

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

                            if (nextChar != '(')
                                throw new ParseException("Expected '(' after foreach. Line number: " + GetLineNumberFromIndex(template, index));
                            
                            StringBuilder consumer = new StringBuilder();
                            index++;
                            while (nextChar != ')')
                            {
                                nextChar = template[index + 1];
                                if (nextChar == '\r' || nextChar == '\n')
                                    throw new ParseException("Expected ')' but encountered newline after foreach. Line number: " + GetLineNumberFromIndex(template, index));

                                consumer.Append(nextChar);
                                index++;
                            }
                            consumer.RemoveLastCharacter();
                            expression.State = consumer.ToString();
                        }
                        else if (element == "if")
                        {
                            IExpression expression = new IfExpression() { Parent = currentNode };
                            currentNode.Children.Add(expression);
                            currentNode = expression;

                            if (nextChar != '(')
                                throw new ParseException("Expected '(' after if. Line number: " + GetLineNumberFromIndex(template, index));

                            StringBuilder consumer = new StringBuilder();
                            consumer.Append(nextChar);
                            int stackCount = 1;
                            index++;
                            while (stackCount > 0)
                            {
                                nextChar = template[index + 1];
                                if (nextChar == '(')
                                    stackCount--;
                                else if (nextChar == ')')
                                    stackCount--;
                                else if (stackCount > 0 && (nextChar == '\r' || nextChar == '\n'))
                                    throw new ParseException("Expected ')' but encountered newline in if-statement. Line number: " + GetLineNumberFromIndex(template, index));

                                consumer.Append(nextChar);
                                index++;
                            }
                            expression.State = consumer.ToString();
                        }
                        else if (element == "elseif")
                        {
                            IExpression expression = new ElseIfExpression() { Parent = currentNode };
                            currentNode.Children.Add(expression);

                            if (nextChar != '(')
                                throw new ParseException("Expected '(' after if. Line number: " + GetLineNumberFromIndex(template, index));

                            StringBuilder consumer = new StringBuilder();
                            consumer.Append(nextChar);
                            int stackCount = 1;
                            index++;
                            while (stackCount > 0)
                            {
                                nextChar = template[index + 1];
                                if (nextChar == '(')
                                    stackCount--;
                                else if (nextChar == ')')
                                    stackCount--;
                                else if (stackCount > 0 && (nextChar == '\r' || nextChar == '\n'))
                                    throw new ParseException("Expected ')' but encountered newline in elseif-statement. Line number: " + GetLineNumberFromIndex(template, index));

                                consumer.Append(nextChar);
                                index++;
                            }
                            expression.State = consumer.ToString();
                        }
                        else if (element == "else")
                        {
                            IExpression expression = new ElseExpression() { Parent = currentNode };
                            currentNode.Children.Add(expression);

                        }
                        else if (element == "end")
                        {
                            currentNode = currentNode.Parent;

                            if (currentNode == null)
                                throw new ParseException("Did not expect #end here. Line: " + GetLineNumberFromIndex(template, index));
                        }
                        else if (foreachSubItems.Contains(element))
                        {
                            IExpression expression = new ForeachSubExpression() { Parent = currentNode, UsedTag = element };
                            currentNode.Children.Add(expression);
                        }
                        else
                        {
                            elementHandled = false;
                        }
                    }
                    else
                    {
                        elementHandled = false;
                    }

                    if (!elementHandled)
                    {
                        if (nextChar != null)
                            builder.Append(nextChar.Value);
                        builder.Append(c);
                        builder.Append(hashbuildUp.ToString());
                        if (builder.Length > 0)
                        {
                            CreateAndAddTextExpression(currentNode, builder, index);
                        }
                    }
                    
                    hashbuildUp.Clear();
                }
                else if (c == '$')
                {
                    if (builder.Length > 0)
                    {
                        CreateAndAddTextExpression(currentNode, builder, index);
                    }
                    
                    if (index + 1 < template.Length)
                    {
                        char nextChar = template[index + 1];
                        index++;
                        if (nextChar == '{')
                        {
                            StringBuilder consumer = new StringBuilder();
                            
                            while (nextChar != '}')
                            {
                                nextChar = template[index + 1];
                                if (nextChar == '\r' || nextChar == '\n')
                                    throw new Exception();

                                consumer.Append(nextChar);
                                index++;
                            }
                            consumer.RemoveLastCharacter();
                            PrintExpression print = new PrintExpression()
                            {
                                Parent = currentNode,
                                IndexInTemplate = index,
                                State = consumer.ToString()
                            };

                            currentNode.Children.Add(print);
                        }
                        else
                        {
                            builder.Append(c);
                            builder.Append(nextChar);
                        }
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
                else
                {
                    builder.Append(c);
                }
            }

            if (builder.Length > 0)
            {
                CreateAndAddTextExpression(currentNode, builder, template.Length - builder.Length);
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
