namespace MathExpressionParser
{
    using System.Collections.Generic;

    internal struct Operators
    {
        public const string Subtraction = "-";
        public const string Addition = "+";
        public const string Division = "/";
        public const string Multiplication = "*";
        public const string Power = "^";
        public const string LeftParenthesis = "(";
        public const string RightParenthesis = ")";
        public const string Modulo = "%";
        public static readonly char[] ValidChars = "0123456789+-)(*^/%.".ToCharArray();
        public static readonly IReadOnlyDictionary<string, int> PrecedenceDict = new Dictionary<string, int>()
            {
                { LeftParenthesis, 10 },
                { RightParenthesis, 10 },
                { Power, 5 },
                { Multiplication, 5 },
                { Division, 5 },
                { Modulo, 5 },
                { Subtraction, 1 },
                { Addition, 1 },
            };
    }
}
