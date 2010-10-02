using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LemurLang.Exceptions;
using LemurLang.Interfaces;
using LemurLang.Templates;

namespace LemurLang
{
    public delegate ITemplate TemplateItemCreator(string elementName);
    
    public class TemplateEngine
    {
        private Dictionary<string, TemplateItemCreator> itemCreators;
        
        public TemplateEngine()
        {
            itemCreators = new Dictionary<string, TemplateItemCreator>();

            //foreach
            this.RegisterItem("foreach", (x) => new ForeachTemplate());
            //foreach subitems
            this.RegisterItem("beforeall", (x) => new ForeachSubTemplate());
            this.RegisterItem("before", (x) => new ForeachSubTemplate());
            this.RegisterItem("odd", (x) => new ForeachSubTemplate());
            this.RegisterItem("even", (x) => new ForeachSubTemplate());
            this.RegisterItem("each", (x) => new ForeachSubTemplate());
            this.RegisterItem("after", (x) => new ForeachSubTemplate());
            this.RegisterItem("between", (x) => new ForeachSubTemplate());
            this.RegisterItem("afterall", (x) => new ForeachSubTemplate());
            this.RegisterItem("nodata", (x) => new ForeachSubTemplate());

            //conditionals
            this.RegisterItem("if", (x) => new IfTemplate());
            this.RegisterItem("elseif", (x) => new ElseIfTemplate());
            this.RegisterItem("else", (x) => new ElseTemplate());
        }

        public void RegisterItem(string name, TemplateItemCreator itemCreator)
        {
            itemCreators[name] = itemCreator;
        }
        
        public RootTemplate BuildTemplate(string template)
        {
            RootTemplate root = new RootTemplate();
            ITemplate currentItem = root;

            StringBuilder builder = new StringBuilder(500);
            StringBuilder hashbuildUp = new StringBuilder(500);

            for(int index = 0; index < template.Length; index++)
            {
                char c = template[index];

                if (c == '#')
                {
                    hashbuildUp.Clear();
                    
                    char nextChar = '\0';

                    bool first = true;
                    while (index + 1 < template.Length)
                    {
                        nextChar = template[ index + 1];
                        if (first && nextChar == '#')//## single line comment
                        {
                            index++;
                            //search for line ending
                            while ((nextChar != '\n' && nextChar != '\r') && index + 1 < template.Length)
                            {
                                index++;
                                nextChar = template[index];
                            }
                            //eat whitespace
                            while ((nextChar == '\n' || nextChar == '\r') && index + 1 < template.Length)
                            {
                                index++;
                                nextChar = template[index];
                            }
                            if (nextChar != '\n' && nextChar != '\r')
                                index--;
                            //
                            break;
                        }
                        else if (first && nextChar == '*') //multiline comment
                        {
                            int startIndex = index;
                            string nextTwoCharacters = null;
                            index++;
                            bool foundCommendEnd = false;
                            while (!foundCommendEnd && (nextTwoCharacters != "*#") && index + 2 < template.Length)
                            {
                                index++;
                                nextTwoCharacters = template.Substring(index, 2);
                                if(nextTwoCharacters == "*#")
                                    foundCommendEnd = true;
                            }
                            index++;

                            if (index + 1 < template.Length)
                            {
                                //eat whitespace
                                index++;
                                nextChar = template[index];
                                while ((nextChar == '\n' || nextChar == '\r') && index + 1 < template.Length)
                                {
                                    index++;
                                    nextChar = template[index];
                                }
                                if (nextChar != '\n' && nextChar != '\r')
                                    index--;
                            }
                            if (!foundCommendEnd)
                                throw new ParseException("Non-ending comment found on line: " + GetLineNumberFromIndex(template, startIndex));

                            
                            break;
                        }

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
                        first = false;
                    }

                    string element = hashbuildUp.ToString();

                    if (builder.Length > 0)
                    {
                        CreateAndAddTextTemplate(currentItem, builder, index);
                    }

                    if (element == "end")
                    {
                        currentItem = currentItem.Parent;

                        if (currentItem == null)
                            throw new ParseException("Did not expect #end here. Line: " + GetLineNumberFromIndex(template, index));

                        //eat whitespace
                        if (index + 1 < template.Length)
                        {
                            index++;
                            nextChar = template[index];
                            while ((nextChar == '\n' || nextChar == '\r') && index + 1 < template.Length)
                            {
                                index++;
                                nextChar = template[index];
                            }
                            if (nextChar != '\n' && nextChar != '\r')
                                index--; 
                        }
                        //
                    }
                    else if (hashbuildUp.Length > 0 && itemCreators.ContainsKey(element))
                    {
                        ITemplate templateItem = itemCreators[element](element);
                        templateItem.Parent = currentItem;
                        templateItem.IndexInTemplate = index;
                        templateItem.UsedTag = element;

                        try
                        {
                            TemplateParseResult result = templateItem.Parse(template, currentItem, index, nextChar);
                            currentItem = result.CurrentTemplate;
                            index = result.Index;
                        }
                        catch (Exception ex)
                        {
                            throw new ParseException("Error occurred while parsing. Line number: " + GetLineNumberFromIndex(template, index), ex);
                        }

                        //eat whitespace
                        nextChar = template[index];
                        while ((nextChar == '\n' || nextChar == '\r') && index + 1 < template.Length)
                        {
                            index++;
                            nextChar = template[index];
                        }
                        if (nextChar != '\n' && nextChar != '\r')
                            index--;
                        //
                    }
                }
                else if (c == '$')
                {
                    if (builder.Length > 0)
                    {
                        CreateAndAddTextTemplate(currentItem, builder, index);
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
                                    throw new ParseException(string.Format("Did not expect {0} here. Line: {1}", Regex.Escape(nextChar.ToString()), GetLineNumberFromIndex(template, index)));

                                consumer.Append(nextChar);
                                index++;
                            }
                            consumer.RemoveLastCharacter();
                            PrintTemplate print = new PrintTemplate()
                            {
                                Parent = currentItem,
                                IndexInTemplate = index,
                                Arguments = consumer.ToString()
                            };

                            currentItem.Children.Add(print);
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
                CreateAndAddTextTemplate(currentItem, builder, template.Length - builder.Length);
            }

            if (currentItem != root)
            {
                int lineNumber = GetLineNumberFromIndex(template, currentItem.IndexInTemplate);
                throw new ParseException(
                    string.Format("Tag '{0}' needs to be ended. Hint -->\r\nLine number: {1}",
                        currentItem.UsedTag,
                        lineNumber
                    )
                );
            }

            hashbuildUp.Clear();

            return root;
        }

        private static void CreateAndAddTextTemplate(ITemplate currentNode, StringBuilder builder, int index)
        {
            TextTemplate text = new TextTemplate()
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
