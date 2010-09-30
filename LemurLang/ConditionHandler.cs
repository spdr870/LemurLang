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

                    if (currentElementList.Children.Last() is LogicalOperatorConditionElement)
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

                        if (currentElementList.Children.Count == 0 || currentElementList.Children.Last() is LogicalOperatorConditionElement)
                        {
                            throw new Exception();//todo
                        }
                        
                        LogicalOperatorConditionElement operatorConditionElement = new LogicalOperatorConditionElement(currentElementList.Parent, "&&");
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

                        if (currentElementList.Children.Count == 0 || currentElementList.Children.Last() is LogicalOperatorConditionElement)
                        {
                            throw new Exception();//todo
                        }

                        LogicalOperatorConditionElement operatorConditionElement = new LogicalOperatorConditionElement(currentElementList.Parent, "||");
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
