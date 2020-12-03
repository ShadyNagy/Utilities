using System.Linq.Expressions;
using ShadyNagy.Utilities.Extensions.Object;

namespace ShadyNagy.Utilities.Extensions.Expressions
{
    public static class ExpressionExtensions
    {
        public static MemberExpression GetNestedProperty(this Expression parameter, string property)
        {
            var propertyNames = property.Split('.');
            var propertyNameExpr = parameter.GetPropertyName(propertyNames[0]);
            if (string.IsNullOrEmpty(propertyNameExpr))
            {
                return null;
            }
            var propertyExpr = Expression.Property(parameter, propertyNameExpr);

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
