using System.Linq.Expressions;

namespace ShadyNagy.Utilities.Extensions.Expressions
{
    public static class ExpressionExtensions
    {
        public static MemberExpression GetNestedProperty(this Expression param, string property)
        {
            var propNames = property.Split('.');
            var propExpr = Expression.Property(param, propNames[0]);

            for (var i = 1; i < propNames.Length; i++)
            {
                propExpr = Expression.Property(propExpr, propNames[i]);
            }

            return propExpr;
        }

    }
}
