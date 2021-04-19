namespace MathExpressionParser.Exceptions
{
    using System;

    public class ExpressionInvalidCharsException : Exception
    {
        public ExpressionInvalidCharsException()
            : base("Invalid char(s) detected")
        {
        }
    }
}
