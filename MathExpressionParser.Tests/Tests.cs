namespace MathExpressionParser.Tests
{
    using System;
    using MathExpressionParser;
    using MathExpressionParser.Exceptions;
    using NUnit.Framework;

    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("1+2*(3+2)", new string[] { "1", "2", "3", "2", "+", "*", "+" })]
        [TestCase("5*6-3*2*3", new string[] { "5", "6", "*", "3", "2", "*", "3", "*", "-" })]
        [TestCase("51*6-33*2*38", new string[] { "51", "6", "*", "33", "2", "*", "38", "*", "-" })]
        [TestCase("51*(6-33)*2*38", new string[] { "51", "6", "33", "-", "*", "2", "*", "38", "*" })]
        [TestCase("1+2.5*(3-16.7*4)/15+7.8", new string[] { "1", "2.5", "3", "16.7", "4", "*", "-", "*", "15", "/", "+", "7.8", "+" })]
        [TestCase("13/6*4-78+2.17*3-3.8*(6+46/3)", new string[] { "13", "6", "/", "4", "*", "78", "-", "2.17", "3", "*", "+", "3.8", "6", "46", "3", "/", "+", "*", "-" })]
        [TestCase("2^0", new string[] { "2", "0", "^" })]
        [TestCase("2+2^3", new string[] { "2", "2", "3", "^", "+" })]
        [TestCase("18-6+3^4-(12-8)", new string[] { "18", "6", "-", "3", "4", "^", "+", "12", "8", "-", "-" })]
        [TestCase("18-6+3^4-(12-8)^3", new string[] { "18", "6", "-", "3", "4", "^", "+", "12", "8", "-", "3", "^", "-" })]
        [TestCase("(19-8)^(4*2)", new string[] { "19", "8", "-", "4", "2", "*", "^" })]
        [TestCase("-5+2", new string[] { "5", "-", "2", "+" })]
        [TestCase("((-5))", new string[] { "5", "-" })]
        [TestCase("()5", new string[] { "5" })]
        [TestCase("6%2", new string[] { "6", "2", "%" })]
        [TestCase("6%3", new string[] { "6", "3", "%" })]
        [TestCase("6%4", new string[] { "6", "4", "%" })]
        [TestCase("6.5+3,5", new string[] { "6.5", "3.5", "+" })]
        [TestCase("6,5+3.5", new string[] { "6.5", "3.5", "+" })]
        public void ConvertFromInfixToPostfixNotation(string expression, string[] expected)
        {
            string[] actual = MathProcessor.ConvertExpressionToPostfixNotation(expression);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("1+2*(3+2)", 11)]
        [TestCase("5*6-3*2*3", 12)]
        [TestCase("51*6-33*2*38", -2202)]
        [TestCase("51*(6-33)*2*38", -104652)]
        [TestCase("1+2.5*(3-16.2*4)/15+7", -2.3)]
        [TestCase("13/6*4-78+2.17*3-3.8*(6+46/3)", -143.89)]
        [TestCase("2^0", 1)]
        [TestCase("2+2^3", 10)]
        [TestCase("18-6+3^4-(12-8)", 89)]
        [TestCase("18-6+3^4-(12-8)^3", 29)]
        [TestCase("(19-8)^(4*2)", 214358881)]
        [TestCase("-5+2", -3)]
        [TestCase("((-5))", -5)]
        [TestCase("()5", 5)]
        [TestCase(" ()5", 5)]
        [TestCase(" () 5", 5)]
        [TestCase("6%2", 0)]
        [TestCase("6%3", 0)]
        [TestCase("6%4", 2)]
        [TestCase("6.5+3,5", 10)]
        [TestCase("6,5+3.5", 10)]
        [TestCase("2^12", 4096)]
        [TestCase("165.49+175.51", 341)]
        [TestCase("65.47+75.49", 140.96)]
        [TestCase("165.47+175.49", 340.96)]
        [TestCase("423.47589-5.00367", 418.47222)]
        [TestCase("423.47589-5.00367*12-8.467+2456.000013", 2810.964863)]
        [TestCase("0.01028560089 - 12.00005007", -11.98976446911)]
        public void CalculateValues(string expression, decimal expected)
        {
            decimal actual = MathProcessor.Calculate(expression);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("((-5)")]
        [TestCase("((5)")]
        [TestCase("(-5))")]
        [TestCase("(5))")]
        [TestCase("5))")]
        [TestCase("((5")]
        [TestCase(")(5")]
        [TestCase(")(")]
        public void MismatchedParenthesisException(string expression)
        {
            Assert.Throws<ExpressionMismatchedParenthesisException>(() => MathProcessor.Calculate(expression));
        }

        [Test]
        public void DivideByZeroException()
        {
            Assert.Throws<DivideByZeroException>(() => MathProcessor.Calculate("1/0"));
        }

        [Test]
        public void ModuloBehaviourUndefinedException()
        {
            Assert.Throws<ModuloBehaviourUndefinedException>(() => MathProcessor.Calculate("6%0"));
        }

        [TestCase("5-9#7")]
        [TestCase("5-9&7")]
        [TestCase("5-9$7")]
        [TestCase("5-9!7")]
        public void ExpressionInvalidCharsException(string expression)
        {
            Assert.Throws<ExpressionInvalidCharsException>(() => MathProcessor.Calculate(expression));
        }

        [TestCase("5+)4-2(")]
        [TestCase("5+4-2(")]
        [TestCase("5+)4-2")]
        [TestCase("5+4-2))")]
        [TestCase("(5+4-2")]
        [TestCase("(5+4-2(")]
        [TestCase("5+(4-2(")]
        [TestCase("5+(4-2")]
        public void ExpressionMismatchedParenthesisException(string expression)
        {
            Assert.Throws<ExpressionMismatchedParenthesisException>(() => MathProcessor.Calculate(expression));
        }
    }
}
