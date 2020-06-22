using System.Linq.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{
    internal class ParameterReplacer : ExpressionVisitor
    {
        /// <summary>
        /// The parameter
        /// </summary>
        private readonly ParameterExpression _parameter;

        /// <summary>
        /// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression"></see>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(_parameter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterReplacer" /> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        internal ParameterReplacer(ParameterExpression parameter)
        {
            _parameter = parameter;
        }
    }

}
