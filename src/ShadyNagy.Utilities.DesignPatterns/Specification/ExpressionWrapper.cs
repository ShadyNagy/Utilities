using System.Linq.Expressions;
using ShadyNagy.Utilities.Extensions.Object;

namespace ShadyNagy.Utilities.DesignPatterns.Specification
{
    public static class ExpressionWrapper
    {
        public static MemberExpression Property(Expression expression, string propertyName)
        {
            var propertyNameRead = expression.GetPropertyName(propertyName);
            if (string.IsNullOrEmpty(propertyNameRead))
            {
                return null;
            }

            return Expression.Property(expression, propertyNameRead);
        }
    }
}
