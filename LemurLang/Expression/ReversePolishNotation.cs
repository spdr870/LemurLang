using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LemurLang.Expression
{
    public class ReversePolishNotation
    {
        private Queue<ReversePolishNotationToken> output;
        private Stack<ReversePolishNotationToken> ops;

        public string OriginalExpression { get; private set; }
        public string TransitionExpression { get; private set; }
        public string PostfixExpression { get; private set; }

        public ReversePolishNotation(string expression)
        {
            OriginalExpression = expression;
            TransitionExpression = string.Empty;
            PostfixExpression = string.Empty;
        }

        private void Parse(string Expression, Func<string, object> contextGetter)
        {
            output = new Queue<ReversePolishNotationToken>();
            ops = new Stack<ReversePolishNotationToken>();

            string sBuffer = Expression.ToLower();
            // captures numbers. Anything like 11 or 22.34 is captured
            sBuffer = Regex.Replace(sBuffer, @"(?<number>\d+(\.\d+)?)", " ${number} ");
            // captures these symbols: + - * / ^ ( )
            sBuffer = Regex.Replace(sBuffer, @"(?<ops>[+\-*/^()])", " ${ops} ");
            // captures alphabets. Currently captures
            // the 3 basic trigonometry functions, sine, cosine and tangent
            sBuffer = Regex.Replace(sBuffer, "(?<alpha>(sin|cos|tan))", " ${alpha} ");
            // trims up consecutive spaces and replace it with just one space
            sBuffer = Regex.Replace(sBuffer, @"\s+", " ").Trim();

            // The following chunk captures unary minus operations.
            // 1) We replace every minus sign with the string "MINUS".
            // 2) Then if we find a "MINUS" with a number or constant in front,
            //    then it's a normal minus operation.
            // 3) Otherwise, it's a unary minus operation.

            // Step 1.
            sBuffer = Regex.Replace(sBuffer, "-", "MINUS");
            // Step 2. Looking for generic number \d+(\.\d+)?
            sBuffer = Regex.Replace(sBuffer, @"(?<number>((\d+(\.\d+)?)))\s+MINUS", "${number} -");
            // Step 3. Use the tilde ~ as the unary minus operator
            sBuffer = Regex.Replace(sBuffer, "MINUS", "~");

            TransitionExpression = sBuffer;

            // tokenise it!
            string[] saParsed = sBuffer.Split(" ".ToCharArray());
            int i = 0;
            Decimal tokenvalue;
            ReversePolishNotationToken token, opstoken;
            for (i = 0; i < saParsed.Length; ++i)
            {
                token = new ReversePolishNotationToken();
                token.TokenValue = saParsed[i];
                token.TokenValueType = TokenType.None;

                try
                {
                    tokenvalue = Decimal.Parse(saParsed[i]);
                    token.TokenValueType = TokenType.Number;
                    // If the token is a number, then add it to the output queue.
                    output.Enqueue(token);
                }
                catch
                {
                    switch (saParsed[i])
                    {
                        case "+":
                            token.TokenValueType = TokenType.Plus;
                            if (ops.Count > 0)
                            {
                                opstoken = ops.Peek();
                                // while there is an operator, o2, at the top of the stack
                                while (IsOperatorToken(opstoken.TokenValueType))
                                {
                                    // pop o2 off the stack, onto the output queue;
                                    output.Enqueue(ops.Pop());
                                    if (ops.Count > 0)
                                    {
                                        opstoken = ops.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            // push o1 onto the operator stack.
                            ops.Push(token);
                            break;
                        case "-":
                            token.TokenValueType = TokenType.Minus;
                            if (ops.Count > 0)
                            {
                                opstoken = ops.Peek();
                                // while there is an operator, o2, at the top of the stack
                                while (IsOperatorToken(opstoken.TokenValueType))
                                {
                                    // pop o2 off the stack, onto the output queue;
                                    output.Enqueue(ops.Pop());
                                    if (ops.Count > 0)
                                    {
                                        opstoken = ops.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            // push o1 onto the operator stack.
                            ops.Push(token);
                            break;
                        case "*":
                            token.TokenValueType = TokenType.Multiply;
                            if (ops.Count > 0)
                            {
                                opstoken = ops.Peek();
                                // while there is an operator, o2, at the top of the stack
                                while (IsOperatorToken(opstoken.TokenValueType))
                                {
                                    if (opstoken.TokenValueType == TokenType.Plus || opstoken.TokenValueType == TokenType.Minus)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        // Once we're in here, the following algorithm condition is satisfied.
                                        // o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or
                                        // o1 is right-associative and its precedence is less than (lower precedence) that of o2,

                                        // pop o2 off the stack, onto the output queue;
                                        output.Enqueue(ops.Pop());
                                        if (ops.Count > 0)
                                        {
                                            opstoken = ops.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            // push o1 onto the operator stack.
                            ops.Push(token);
                            break;
                        case "/":
                            token.TokenValueType = TokenType.Divide;
                            if (ops.Count > 0)
                            {
                                opstoken = ops.Peek();
                                // while there is an operator, o2, at the top of the stack
                                while (IsOperatorToken(opstoken.TokenValueType))
                                {
                                    if (opstoken.TokenValueType == TokenType.Plus || opstoken.TokenValueType == TokenType.Minus)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        // Once we're in here, the following algorithm condition is satisfied.
                                        // o1 is associative or left-associative and its precedence is less than (lower precedence) or equal to that of o2, or
                                        // o1 is right-associative and its precedence is less than (lower precedence) that of o2,

                                        // pop o2 off the stack, onto the output queue;
                                        output.Enqueue(ops.Pop());
                                        if (ops.Count > 0)
                                        {
                                            opstoken = ops.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            // push o1 onto the operator stack.
                            ops.Push(token);
                            break;
                        case "^":
                            token.TokenValueType = TokenType.Exponent;
                            // push o1 onto the operator stack.
                            ops.Push(token);
                            break;
                        case "~":
                            token.TokenValueType = TokenType.UnaryMinus;
                            // push o1 onto the operator stack.
                            ops.Push(token);
                            break;
                        case "(":
                            token.TokenValueType = TokenType.LeftParenthesis;
                            // If the token is a left parenthesis, then push it onto the stack.
                            ops.Push(token);
                            break;
                        case ")":
                            token.TokenValueType = TokenType.RightParenthesis;
                            if (ops.Count > 0)
                            {
                                opstoken = ops.Peek();
                                // Until the token at the top of the stack is a left parenthesis
                                while (opstoken.TokenValueType != TokenType.LeftParenthesis)
                                {
                                    // pop operators off the stack onto the output queue
                                    output.Enqueue(ops.Pop());
                                    if (ops.Count > 0)
                                    {
                                        opstoken = ops.Peek();
                                    }
                                    else
                                    {
                                        // If the stack runs out without finding a left parenthesis,
                                        // then there are mismatched parentheses.
                                        throw new Exception("Unbalanced parenthesis!");
                                    }

                                }
                                // Pop the left parenthesis from the stack, but not onto the output queue.
                                ops.Pop();
                            }

                            if (ops.Count > 0)
                            {
                                opstoken = ops.Peek();
                                // If the token at the top of the stack is a function token
                                if (IsFunctionToken(opstoken.TokenValueType))
                                {
                                    // pop it and onto the output queue.
                                    output.Enqueue(ops.Pop());
                                }
                            }
                            break;
                    }
                }
            }

            // When there are no more tokens to read:

            // While there are still operator tokens in the stack:
            while (ops.Count != 0)
            {
                opstoken = ops.Pop();
                // If the operator token on the top of the stack is a parenthesis
                if (opstoken.TokenValueType == TokenType.LeftParenthesis)
                {
                    // then there are mismatched parenthesis.
                    throw new Exception("Unbalanced parenthesis!");
                }
                else
                {
                    // Pop the operator onto the output queue.
                    output.Enqueue(opstoken);
                }
            }

            PostfixExpression = string.Empty;
            foreach (ReversePolishNotationToken obj in output)
            {
                opstoken = obj;
                PostfixExpression += string.Format("{0} ", opstoken.TokenValue);
            }
        }

        public Decimal Evaluate(Func<string, object> contextGetter)
        {
            Parse(OriginalExpression, contextGetter);

            Stack<Decimal> result = new Stack<Decimal>();
            Decimal operator1 = 0.0m;
            Decimal operator2 = 0.0m;
            // While there are input tokens left
            foreach (ReversePolishNotationToken token in output)
            {
                // Read the next token from input.
                switch (token.TokenValueType)
                {
                    case TokenType.Number:
                        // If the token is a value
                        // Push it onto the stack.
                        result.Push(Decimal.Parse(token.TokenValue));
                        break;
                    case TokenType.Constant:
                        // If the token is a value
                        // Push it onto the stack.
                        result.Push(Convert.ToDecimal(contextGetter(token.TokenValue)));
                        break;
                    case TokenType.Plus:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // So, pop the top n values from the stack.
                            operator2 = result.Pop();
                            operator1 = result.Pop();
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(operator1 + operator2);
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Minus:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // So, pop the top n values from the stack.
                            operator2 = result.Pop();
                            operator1 = result.Pop();
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(operator1 - operator2);
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Multiply:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // So, pop the top n values from the stack.
                            operator2 = result.Pop();
                            operator1 = result.Pop();
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(operator1 * operator2);
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Divide:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // So, pop the top n values from the stack.
                            operator2 = result.Pop();
                            operator1 = result.Pop();
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(operator1 / operator2);
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Exponent:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // So, pop the top n values from the stack.
                            operator2 = result.Pop();
                            operator1 = result.Pop();
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push((Decimal)Math.Pow((double)operator1, (double)operator2));
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.UnaryMinus:
                        // NOTE: n is 1 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 1)
                        {
                            // So, pop the top n values from the stack.
                            operator1 = result.Pop();
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(-operator1);
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                }
            }

            // If there is only one value in the stack
            if (result.Count == 1)
            {
                // That value is the result of the calculation.
                return result.Pop();
            }
            else
            {
                // If there are more values in the stack
                // (Error) The user input too many values.
                throw new Exception("Evaluation error!");
            }
        }

        private bool IsOperatorToken(TokenType t)
        {
            bool result = false;
            switch (t)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Multiply:
                case TokenType.Divide:
                case TokenType.Exponent:
                case TokenType.UnaryMinus:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        private bool IsFunctionToken(TokenType t)
        {
            //functions are not supported yet
            return false;
        }
    }
}