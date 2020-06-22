using System;
using System.Linq;
using System.Linq.Expressions;

namespace ShadyNagy.Utilities.Extensions
{

    public static class QueryableExtensions
    {

        public static T FirstOrDefaultPropertyName<T>(this IQueryable<T> source, string propertyName, object value)
        {
            return source.FirstOrDefault(ToLambda2<T>(propertyName, value));
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }


        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, bool>> ToLambda2<T>(string propertyName, object value)
        {
            if (string.IsNullOrEmpty(propertyName) || !IsPropertyExist<T>(propertyName))
            {
                propertyName = GetFirstProperty<T>();
            }

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, property.Type);
            var toCompareTo = Expression.Constant(value, property.Type);

            Expression equalsMethod = Expression.Call(propAsObject, "Equals", null, toCompareTo);
            return Expression.Lambda<Func<T, bool>>(equalsMethod, parameter);
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !IsPropertyExist<T>(propertyName))
            {
                propertyName = GetFirstProperty<T>();
            }

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        private static bool IsPropertyExist<T>(string propertyName)
        {

            var prop = typeof(T).GetProperty(propertyName);

            return prop != null;
        }

        private static bool IsPropertyExist(Type typeToCheck, string propertyName)
        {

            var prop = typeToCheck.GetProperty(propertyName);

            return prop != null;
        }

        private static string GetFirstProperty<T>()
        {
            var props = typeof(T).GetProperties();
            return props[0].Name;
        }
    }

}
