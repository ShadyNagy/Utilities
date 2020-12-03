using System.Linq.Expressions;
using ShadyNagy.Utilities.Extensions.Object;

namespace ShadyNagy.Utilities.Extensions.Expressions
{
    public static class ExpressionExtensions
    {
        public static MemberExpression GetNestedProperty(this Expression parameter, string property)
        {
            var propertyNames = property.Split('.');
            var propertyExpr = Expression.Property(parameter, propertyNames[0]);

            for (var i = 1; i < propertyNames.Length; i++)
            {
                var propertyName = parameter.GetPropertyName(propertyNames[i]);
                if (string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }
                propertyExpr = Expression.Property(propertyExpr, propertyName);
            }

            return propertyExpr;
        }

    }
}
