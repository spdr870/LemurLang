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

            TransitionExpression = Expression;

            // tokenise it!
            string[] saParsed = Regex.Split(Expression, @"([+\-*/^()]|==|!=|>=|>|<=|<|\|\||\&\&|!)", RegexOptions.Multiline);

            int i = 0;
            ReversePolishNotationToken opstoken;
            ReversePolishNotationToken lastToken;
            lastToken.TokenValueType = TokenType.None;

            for (i = 0; i < saParsed.Length; ++i)
            {
                if (string.IsNullOrEmpty(saParsed[i].Trim()))
                    continue;

                ReversePolishNotationToken token = new ReversePolishNotationToken();
                token.TokenValue = saParsed[i].Trim();
                token.TokenValueType = TokenType.None;

                switch (saParsed[i].Trim())
                {
                    default:
                        token.TokenValueType = TokenType.Number;
                        // If the token is a number, then add it to the output queue.
                        output.Enqueue(token);
                        break;
                    case "||":
                        token.TokenValueType = TokenType.LogicalOr;
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
                    case "!":
                        token.TokenValueType = TokenType.Not;
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
                    case "&&":
                        token.TokenValueType = TokenType.LogicalAnd;
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
                    case "==":
                        token.TokenValueType = TokenType.Equals;
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
                    case "!=":
                        token.TokenValueType = TokenType.NotEquals;
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
                    case ">":
                        token.TokenValueType = TokenType.GreaterThan;
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
                    case ">=":
                        token.TokenValueType = TokenType.GreaterThanOrEquals;
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
                    case "<":
                        token.TokenValueType = TokenType.SmallerThan;
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
                    case "<=":
                        token.TokenValueType = TokenType.SmallerThanOrEquals;
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
                    case "+":
                        token.TokenValueType = TokenType.Plus;
                        if (ops.Count > 0)
                        {
                            opstoken = ops.Peek();
                            // while there is an operator, o2, at the top of the stack
                            while (IsOperatorToken(opstoken.TokenValueType))
                            {
                                if (opstoken.TokenValueType == TokenType.Equals || opstoken.TokenValueType == TokenType.NotEquals || opstoken.TokenValueType == TokenType.GreaterThan || opstoken.TokenValueType == TokenType.GreaterThanOrEquals || opstoken.TokenValueType == TokenType.SmallerThan || opstoken.TokenValueType == TokenType.SmallerThanOrEquals || opstoken.TokenValueType == TokenType.LogicalAnd || opstoken.TokenValueType == TokenType.LogicalOr || opstoken.TokenValueType == TokenType.Not)
                                {
                                    break;
                                }
                                else
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
                        }
                        // push o1 onto the operator stack.
                        ops.Push(token);
                        break;
                    case "-":
                        if (lastToken.TokenValueType == TokenType.None || IsOperatorToken(lastToken.TokenValueType))
                        {
                            token.TokenValueType = TokenType.UnaryMinus;
                            // push o1 onto the operator stack.
                            ops.Push(token);
                            break;
                        } else
                        {
                            token.TokenValueType = TokenType.Minus;
                            if (ops.Count > 0)
                            {
                                opstoken = ops.Peek();
                                // while there is an operator, o2, at the top of the stack
                                while (IsOperatorToken(opstoken.TokenValueType))
                                {
                                    if (opstoken.TokenValueType == TokenType.Equals || opstoken.TokenValueType == TokenType.NotEquals || opstoken.TokenValueType == TokenType.GreaterThan || opstoken.TokenValueType == TokenType.GreaterThanOrEquals || opstoken.TokenValueType == TokenType.SmallerThan || opstoken.TokenValueType == TokenType.SmallerThanOrEquals || opstoken.TokenValueType == TokenType.LogicalAnd || opstoken.TokenValueType == TokenType.LogicalOr || opstoken.TokenValueType == TokenType.Not)
                                    {
                                        break;
                                    }
                                    else
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
                            }
                            // push o1 onto the operator stack.
                            ops.Push(token);
                            break;
                        }
                    case "*":
                        token.TokenValueType = TokenType.Multiply;
                        if (ops.Count > 0)
                        {
                            opstoken = ops.Peek();
                            // while there is an operator, o2, at the top of the stack
                            while (IsOperatorToken(opstoken.TokenValueType))
                            {
                                if (opstoken.TokenValueType == TokenType.Plus || opstoken.TokenValueType == TokenType.Minus || opstoken.TokenValueType == TokenType.Equals || opstoken.TokenValueType == TokenType.NotEquals || opstoken.TokenValueType == TokenType.GreaterThan || opstoken.TokenValueType == TokenType.GreaterThanOrEquals || opstoken.TokenValueType == TokenType.SmallerThan || opstoken.TokenValueType == TokenType.SmallerThanOrEquals || opstoken.TokenValueType == TokenType.LogicalAnd || opstoken.TokenValueType == TokenType.LogicalOr || opstoken.TokenValueType == TokenType.Not)
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
                                if (opstoken.TokenValueType == TokenType.Plus || opstoken.TokenValueType == TokenType.Minus || opstoken.TokenValueType == TokenType.GreaterThan || opstoken.TokenValueType == TokenType.GreaterThanOrEquals || opstoken.TokenValueType == TokenType.SmallerThan || opstoken.TokenValueType == TokenType.SmallerThanOrEquals || opstoken.TokenValueType == TokenType.LogicalAnd || opstoken.TokenValueType == TokenType.LogicalOr || opstoken.TokenValueType == TokenType.Not)
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

                lastToken = token;
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

        private object Resolve(object input, Func<string, object> contextGetter)
        {
            Regex contextProperty = new Regex(@"^\$\{([a-zA-Z0-9.]+)\}$");

            Match lhsMatch = contextProperty.Match(input.ToString());
            return lhsMatch.Success ? contextGetter(lhsMatch.Groups[1].Value) : input;
        }

        public object Evaluate(Func<string, object> contextGetter)
        {
            Parse(OriginalExpression, contextGetter);

            Stack result = new Stack();
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
                        result.Push(token.TokenValue);
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
                            operator2 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
                            operator1 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
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
                            operator2 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
                            operator1 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
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
                            operator2 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
                            operator1 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
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
                            operator2 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
                            operator1 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
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
                            operator2 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
                            operator1 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
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
                            operator1 = Convert.ToDecimal(Resolve(result.Pop(), contextGetter));
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
                    case TokenType.Not:
                        // NOTE: n is 1 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 1)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(!Convert.ToBoolean(Resolve(result.Pop(), contextGetter)));
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.LogicalAnd:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            bool left = Convert.ToBoolean(Resolve(result.Pop(), contextGetter));
                            bool right = Convert.ToBoolean(Resolve(result.Pop(), contextGetter));
                            result.Push(left && right); //Pop them before this line or change is one of them is not popped
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.LogicalOr:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            bool left = Convert.ToBoolean(Resolve(result.Pop(), contextGetter));
                            bool right = Convert.ToBoolean(Resolve(result.Pop(), contextGetter));
                            result.Push(left || right);//Pop them before this line or change is one of them is not popped
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.Equals:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(Resolve(result.Pop(), contextGetter).ToString() == Resolve(result.Pop(), contextGetter).ToString());
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.NotEquals:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(!(Resolve(result.Pop(), contextGetter).ToString() == Resolve(result.Pop(), contextGetter).ToString()));
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.GreaterThan:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(Convert.ToDecimal(Resolve(result.Pop(), contextGetter)) < Convert.ToDecimal(Resolve(result.Pop(), contextGetter)));
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.GreaterThanOrEquals:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(Convert.ToDecimal(Resolve(result.Pop(), contextGetter)) <= Convert.ToDecimal(Resolve(result.Pop(), contextGetter)));
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.SmallerThan:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(Convert.ToDecimal(Resolve(result.Pop(), contextGetter)) > Convert.ToDecimal(Resolve(result.Pop(), contextGetter)));
                        }
                        else
                        {
                            // (Error) The user has not input sufficient values in the expression.
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenType.SmallerThanOrEquals:
                        // NOTE: n is 2 in this case
                        // If there are fewer than n values on the stack
                        if (result.Count >= 2)
                        {
                            // Evaluate the function, with the values as arguments.
                            // Push the returned results, if any, back onto the stack.
                            result.Push(Convert.ToDecimal(Resolve(result.Pop(), contextGetter)) >= Convert.ToDecimal(Resolve(result.Pop(), contextGetter)));
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
                case TokenType.Equals:
                case TokenType.NotEquals:
                case TokenType.Not:
                case TokenType.GreaterThan:
                case TokenType.GreaterThanOrEquals:
                case TokenType.SmallerThan:
                case TokenType.SmallerThanOrEquals:
                case TokenType.LogicalOr:
                case TokenType.LogicalAnd:
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