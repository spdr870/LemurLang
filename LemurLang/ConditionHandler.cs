using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LemurLang.Conditions;

namespace LemurLang
{
    public class ConditionHandler
    {
        private void Clear(StringBuilder builder)
        {
            builder.Remove(0, builder.Length);
        }

        private void RemoveLastCharacter(StringBuilder builder)
        {
            builder.Remove(builder.Length-1, builder.Length);
        }

        public ConditionElementList Build(string input)
        {
            string workstring = input;

            ConditionElementList rootElementList = null;
            ConditionElementList currentElementList = null;

            StringBuilder currentLiteral = new StringBuilder();

            if (!Regex.IsMatch(workstring, @"^\("))
            {
                workstring = string.Concat("(", workstring, ")");
            }

            bool lastCharacterWasAmpersand = false;
            bool lastCharacterWasPipe = false;

            foreach (char currentChar in workstring)
            {
                if (currentChar == '(') //start new list
                {
                    if (rootElementList == null) // first list
                    {
                        currentElementList = rootElementList = new ConditionElementList(null);
                    }
                    else //previous list was present
                    {
                        AddPossibleText(currentElementList, currentLiteral);
                        
                        //create new list
                        ConditionElementList newList = new ConditionElementList(currentElementList);
                        
                        //add new list to previous list
                        currentElementList.Children.Add(newList);
                        //the new list is now the current list
                        currentElementList = newList;
                    }
                }
                else if (currentChar == ')')
                {
                    AddPossibleText(currentElementList, currentLiteral);

                    if (currentElementList.Children.Last() is OperatorConditionElement)
                    {
                        throw new Exception();//todo
                    }

                    //pop back to previous list
                    currentElementList = (ConditionElementList)currentElementList.Parent;
                }
                else if (currentChar == '&')
                {
                    if (lastCharacterWasAmpersand)
                    {
                        AddPossibleText(currentElementList, currentLiteral);

                        if (currentElementList.Children.Count == 0 || currentElementList.Children.Last() is OperatorConditionElement)
                        {
                            throw new Exception();//todo
                        }
                        
                        OperatorConditionElement operatorConditionElement = new OperatorConditionElement(currentElementList.Parent, "&&");
                        currentElementList.Children.Add(operatorConditionElement);
                        lastCharacterWasAmpersand = false;
                    }
                    else
                    {
                        lastCharacterWasAmpersand = true;
                    }
                }
                else if (currentChar == '|')
                {
                    if (lastCharacterWasPipe)
                    {
                        AddPossibleText(currentElementList, currentLiteral);

                        if (currentElementList.Children.Count == 0 || currentElementList.Children.Last() is OperatorConditionElement)
                        {
                            throw new Exception();//todo
                        }

                        OperatorConditionElement operatorConditionElement = new OperatorConditionElement(currentElementList.Parent, "||");
                        currentElementList.Children.Add(operatorConditionElement);
                        lastCharacterWasPipe = false;
                    }
                    else
                    {
                        lastCharacterWasPipe = true;
                    }
                }
                else
                {
                    currentLiteral.Append(currentChar);
                }
            }

            return rootElementList;
        }

        private void AddPossibleText(ConditionElementList currentElementList, StringBuilder currentLiteral)
        {
            string text = currentLiteral.ToString().Trim();
            if (text.Length > 0) //previous text was present
            {
                //add new string element to previous list
                currentElementList.Children.Add(new StringConditionElement(currentElementList, text));
                Clear(currentLiteral);
            }
        }
    }
}
