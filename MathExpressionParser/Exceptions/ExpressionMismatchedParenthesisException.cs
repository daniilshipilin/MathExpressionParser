namespace MathExpressionParser.Exceptions
{
    using System;

    public class ExpressionMismatchedParenthesisException : Exception
    {
        public ExpressionMismatchedParenthesisException()
            : base("Mismatched parenthesis detected")
        {
        }
    }
}
