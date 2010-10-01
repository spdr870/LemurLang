using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LemurLang.Conditions;
using LemurLang.Exceptions;

namespace LemurLang
{
    public class ConditionEngine
    {
        public ConditionElementList Build(string input)
        {
            string workstring = input;

            ConditionElementList rootElementList = null;
            ConditionElementList currentElementList = null;

            StringBuilder currentLiteral = new StringBuilder();

            //make safe...
            workstring = string.Concat("(", workstring, ")");

            bool lastCharacterWasAmpersand = false;
            bool lastCharacterWasPipe = false;

            int index = 0;
            int parenthesisStack = 0;
            foreach (char currentChar in workstring)
            {
                index++;
                
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

                    parenthesisStack++;
                }
                else if (currentChar == ')')
                {
                    AddPossibleText(currentElementList, currentLiteral);

                    if (currentElementList == null || (currentElementList.Parent == null && currentElementList != rootElementList) || currentElementList.Children.Last() is LogicalOperatorConditionElement)
                    {
                        throw new ConditionException(string.Format(
                            "Did not expect: '{0}' here. Condition index: {1}",
                            ")",
                            index
                        ));
                    }

                    //pop back to previous list
                    currentElementList = (ConditionElementList)currentElementList.Parent;

                    parenthesisStack--;
                }
                else if (currentChar == '&')
                {
                    if (lastCharacterWasAmpersand)
                    {
                        AddPossibleText(currentElementList, currentLiteral);

                        if (currentElementList.Children.Count == 0 || currentElementList.Children.Last() is LogicalOperatorConditionElement)
                        {
                            throw new ConditionException(string.Format(
                                "Did not expect: '{0}' here. Condition index: {1}",
                                "&",
                                index
                            ));
                        }
                        
                        LogicalOperatorConditionElement operatorConditionElement = new LogicalOperatorConditionElement(currentElementList.Parent, "&&");
                        currentElementList.Children.Add(operatorConditionElement);
                        lastCharacterWasAmpersand = false;
                    }
                    else if (lastCharacterWasPipe)
                    {
                        throw new ConditionException(string.Format(
                            "Did not expect: '{0}' here. Condition index: {1}",
                            "|",
                            index
                        ));
                    }
                    else
                    {
                        if ((currentLiteral.Length == 0 || currentLiteral.ToString().Trim() == string.Empty) && currentElementList.Children.LastOrDefault() is LogicalOperatorConditionElement)
                        {
                            throw new ConditionException(string.Format(
                                "Did not expect: '{0}' here. Condition index: {1}",
                                "&",
                                index
                            ));
                        }
                        
                        lastCharacterWasAmpersand = true;
                    }
                }
                else if (currentChar == '|')
                {
                    if (lastCharacterWasPipe)
                    {
                        AddPossibleText(currentElementList, currentLiteral);

                        if (currentElementList.Children.Count == 0 || currentElementList.Children.Last() is LogicalOperatorConditionElement)
                        {
                            throw new ConditionException(string.Format(
                                "Did not expect: '{0}' here. Condition index: {1}",
                                "|",
                                index
                            ));
                        }

                        LogicalOperatorConditionElement operatorConditionElement = new LogicalOperatorConditionElement(currentElementList.Parent, "||");
                        currentElementList.Children.Add(operatorConditionElement);
                        lastCharacterWasPipe = false;
                    }
                    else if (lastCharacterWasAmpersand)
                    {
                        throw new ConditionException(string.Format(
                            "Did not expect: '{0}' here. Condition index: {1}",
                            "|",
                            index
                        ));
                    }
                    else
                    {
                        if ((currentLiteral.Length == 0 || currentLiteral.ToString().Trim() == string.Empty) && currentElementList.Children.LastOrDefault() is LogicalOperatorConditionElement)
                        {
                            throw new ConditionException(string.Format(
                                "Did not expect: '{0}' here. Condition index: {1}",
                                "|",
                                index
                            ));
                        }

                        lastCharacterWasPipe = true;
                    }
                }
                else
                {
                    currentLiteral.Append(currentChar);
                }
            }

            if (parenthesisStack > 0)
            {
                throw new ConditionException(string.Format("Missing closing parenthesis(es): {0}", parenthesisStack));
            }

            return rootElementList;
        }

        private void AddPossibleText(ConditionElementList currentElementList, StringBuilder currentLiteral)
        {
            string text = currentLiteral.ToString().Trim();
            if (text.Length > 0) //previous text was present
            {
                Match match = Regex.Match(text, @"^(?'lhs'\S+?)(?:\s*)(?'operator'\>=|\<=|\>|\<|==|!=)(?:\s*)(?'rhs'.+)$");
                if (match.Success)
                {
                    //add new string element to previous list
                    currentElementList.Children.Add(new ComparisonConditionElement(
                        currentElementList,
                        match.Groups["lhs"].Value,
                        match.Groups["operator"].Value,
                        match.Groups["rhs"].Value
                    ));
                }
                else
                {
                    //add new string element to previous list
                    currentElementList.Children.Add(new StringConditionElement(currentElementList, text));
                }
                currentLiteral.Clear();
            }
        }
    }
}
