namespace MathExpressionParser.Exceptions
{
    using System;

    public class ModuloBehaviourUndefinedException : Exception
    {
        public ModuloBehaviourUndefinedException()
            : base("a mod 0 is undefined")
        {
        }
    }
}
