using SimpleExpressionEvaluator;

namespace Taschenrechner
{
    /// <summary>
    ///     Diese Klasse rechnet.
    /// </summary>
    internal class Rechner
    {
        internal decimal EvaluateExpression(string equation) => new ExpressionEvaluator().Evaluate(equation);
    }
}