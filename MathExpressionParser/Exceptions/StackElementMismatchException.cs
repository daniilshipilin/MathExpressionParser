namespace MathExpressionParser.Exceptions
{
    using System;

    public class StackElementMismatchException : Exception
    {
        public StackElementMismatchException()
            : base("Stack should only have one value")
        {
        }
    }
}
