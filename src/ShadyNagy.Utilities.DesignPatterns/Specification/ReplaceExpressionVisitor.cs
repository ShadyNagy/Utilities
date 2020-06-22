using System.Linq.Expressions;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{
    internal class ReplaceExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// The old value
        /// </summary>
        private readonly Expression _oldValue;
        /// <summary>
        /// The new value
        /// </summary>
        private readonly Expression _newValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceExpressionVisitor" /> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        /// <summary>
        /// Dispatches the expression to one of the more specialized visit methods in this class.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }

}
