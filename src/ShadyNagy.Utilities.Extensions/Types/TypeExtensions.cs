using System;
using System.Reflection;

namespace ShadyNagy.Utilities.Extensions.Types
{
    public static class TypeExtensions
    {
        public static PropertyInfo GetRuntimePropertyWithoutCase(this Type type, string property)
        {
            var propertiesName = type.GetRuntimeProperties();
            foreach (var propertyName in propertiesName)
            {
                if (string.Equals(property, propertyName.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return type.GetRuntimeProperty(propertyName.Name);
                }
            }

            return null;
        }

        public static string GetRuntimePropertyNameWithoutCase(this Type type, string property)
        {
            var propertiesName = type.GetRuntimeProperties();
            foreach (var propertyName in propertiesName)
            {
                if (string.Equals(property, propertyName.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return propertyName.Name;
                }
            }

            return null;
        }
    }
}
