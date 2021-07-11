namespace MathExpressionParser
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using MathExpressionParser.Exceptions;

    /// <summary>
    /// <see cref="MathProcessor"/> class.
    /// </summary>
    public static class MathProcessor
    {
        /// <summary>
        /// Returns computed expression result.
        /// </summary>
        public static double Calculate(string expression)
        {
            string[] postfix = ConvertExpressionToPostfixNotation(expression);
            double result = EvaluatePostfixNotation(postfix);

            return result;
        }

        /// <summary>
        /// Returns string array in reverse polish notation (RPN) form.
        /// </summary>
        public static string[] ConvertExpressionToPostfixNotation(string expression)
        {
            string[] infix = ValidateAndSplitExpression(expression);

            // empty stack for operators
            var stack = new Stack<string>();

            // reversed polish notation (postfix) - result list
            var rpn = new List<string>();

            // get token (operand or operator)
            for (int i = 0; i < infix.Length; i++)
            {
                if (infix[i] == Operators.LeftParenthesis)
                {
                    // left parenthesis: Push it into stack
                    stack.Push(infix[i]);
                }
                else if (infix[i] == Operators.RightParenthesis)
                {
                    if (stack.Count == 0)
                    {
                        throw new ExpressionMismatchedParenthesisException();
                    }

                    // right parenthesis: Keep popping from the stack and appending to RPN string until we reach the left parenthesis.
                    // if stack becomes empty and we didn't reach the left parenthesis then break out with error
                    while (stack.Count != 0 && stack.Peek() != Operators.LeftParenthesis)
                    {
                        rpn.Add(stack.Pop());
                    }

                    if (stack.Count == 0)
                    {
                        throw new ExpressionMismatchedParenthesisException();
                    }

                    // remove '(' element from the stack
                    stack.Pop();
                }
                else if (Operators.PrecedenceDict.ContainsKey(infix[i]))
                {
                    // Operator: If stack is empty or operator has a higher precedence than the top of the stack then push operator into stack.
                    // Else if operator has lower precedence then we keep popping and appending to RPN string,
                    // this is repeated until operator in stack has lower precedence than the current operator.
                    if (stack.Count == 0 || Operators.PrecedenceDict[infix[i]] > Operators.PrecedenceDict[stack.Peek()])
                    {
                        stack.Push(infix[i]);
                    }
                    else
                    {
                        while (stack.Count != 0 && Operators.PrecedenceDict[infix[i]] <= Operators.PrecedenceDict[stack.Peek()])
                        {
                            if (stack.Peek() is Operators.LeftParenthesis or Operators.RightParenthesis)
                            {
                                break;
                            }

                            rpn.Add(stack.Pop());
                        }

                        stack.Push(infix[i]);
                    }
                }
                else
                {
                    // an operand: we simply append it to RPN string
                    rpn.Add(infix[i]);
                }
            }

            // when the infix expression is finished, we start popping off the stack and appending to RPN string till stack becomes empty
            while (stack.Count > 0)
            {
                if (stack.Peek() is Operators.LeftParenthesis or Operators.RightParenthesis)
                {
                    throw new ExpressionMismatchedParenthesisException();
                }

                rpn.Add(stack.Pop());
            }

            return rpn.ToArray();
        }

        /// <summary>
        /// Validates input expression and then splits it in individual elements.
        /// </summary>
        private static string[] ValidateAndSplitExpression(string expression)
        {
            // remove whitespace chars first, and replace ',' chars with '.'
            string exp = expression.Replace(" ", string.Empty).Replace(",", ".");

            // check for non-valid chars
            foreach (var item in exp)
            {
                if (!Operators.ValidChars.Contains(item))
                {
                    throw new ExpressionInvalidCharsException();
                }
            }

            var list = new List<string>();

            foreach (var match in Regex.Matches(exp, @"([*+/\-)(])|([0-9.]+|.)"))
            {
                if (match is not null)
                {
                    string res = match.ToString() ?? string.Empty;
                    list.Add(res);
                }
            }

            return list.ToArray();
        }

        private static double EvaluatePostfixNotation(string[] postfix)
        {
            // empty stack for storing results
            var stack = new Stack<double>();

            for (int i = 0; i < postfix.Length; i++)
            {
                if (Operators.PrecedenceDict.ContainsKey(postfix[i]))
                {
                    // an operator: get operand from stack and calculate the result
                    double operator2 = stack.Pop();
                    double operator1 = 0;

                    if (stack.Count > 0)
                    {
                        operator1 = stack.Pop();
                    }

                    double result = DoMath(operator1, operator2, postfix[i]);

                    // push the result into the stack
                    stack.Push(result);
                }
                else
                {
                    // an operand: stack its numerical representation into stack
                    stack.Push(double.Parse(postfix[i], CultureInfo.InvariantCulture));
                }
            }

            // at the end of the RPN expression, the stack should only have one value and that should be the result and can be retrieved from the top of the stack.
            if (stack.Count != 1)
            {
                throw new StackElementMismatchException();
            }

            return stack.Pop();
        }

        private static double DoMath(double operator1, double operator2, string operation)
        {
            if (operation == Operators.Power)
            {
                return Math.Pow(operator1, operator2);
            }
            else if (operation == Operators.Multiplication)
            {
                return operator1 * operator2;
            }
            else if (operation == Operators.Division)
            {
                if (operator2 == 0)
                {
                    throw new DivideByZeroException();
                }

                return operator1 / operator2;
            }
            else if (operation == Operators.Modulo)
            {
                if (operator2 == 0)
                {
                    throw new ModuloBehaviourUndefinedException();
                }

                return operator1 % operator2;
            }
            else if (operation == Operators.Subtraction)
            {
                return operator1 - operator2;
            }
            else if (operation == Operators.Addition)
            {
                return operator1 + operator2;
            }
            else
            {
                throw new UnknownOperatorException();
            }
        }
    }
}
