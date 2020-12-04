using System;
using System.Reflection;
using ShadyNagy.Utilities.Extensions.Object;

namespace ShadyNagy.Utilities.Extensions.Types
{
    public static class TypeExtensions
    {
        public static PropertyInfo GetRuntimePropertyWithoutCase(this Type type, string property)
        {
            var propertyWithoutCase = type.GetPropertyName(property);
            if (propertyWithoutCase == null)
            {
                return null;
            }

            return type.GetRuntimeProperty(propertyWithoutCase);
        }
    }
}
