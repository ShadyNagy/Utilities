using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace ShadyNagy.Utilities.Extensions.Object
{

    public static class ObjectExtension
    {

        public static double? ConvertToNullableDouble(this object val)
        {
            if (val == null)
            {
                return null;
            }

            if (!double.TryParse(val.ToString(), out var res))
            {
                return null;
            }

            return res;
        }

        public static int? ConvertToNullableInt(this object val)
        {
            if (val == null)
            {
                return null;
            }

            if (!int.TryParse(val.ToString(), out var res))
            {
                return null;
            }
                

            return res;
        }

        public static long? ConvertToNullableLong(this object val)
        {
            if (val == null)
            {
                return null;
            }

            if (!long.TryParse(val.ToString(), out var res))
            {
                return null;
            }

            return res;
        }

        public static decimal? ConvertToNullableDecimal(this object val)
        {
            if (val == null)
            {
                return null;
            }

            if (!decimal.TryParse(val.ToString(), out var res))
            {
                return null;
            }

            return res;
        }

        public static bool? ConvertToNullableBool(this object val)
        {
            if (val == null)
            {
                return null;
            }

            if (!val.ParseBool(out var res))
            {
                return null;
            }
                

            return res;
        }


        public static DateTime? ConvertToNullableDateTime(this object val, string format=null)
        {
            if (val == null)
            {
                return null;
            }

            DateTime res;

            if (string.IsNullOrEmpty(format))
            {
                if (!DateTime.TryParse(val.ToString(), out res))
                {
                    return null;
                }
            }
            else
            {
                if (!DateTime.TryParseExact(
                    val.ToString(),
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out res))
                {
                    return null;
                }
                    
            }
                

            return res;
        }

        public static Guid? ConvertToNullableGuid(this object val)
        {
            if (val == null)
            {
                return null;
            }

            if (!Guid.TryParse(val.ToString(), out var res))
            {
                return null;
            }

            return res;
        }

        public static bool ParseBool(this object obj, out bool res)
        {
            res = false;

            if (obj == null)
            {
                return false;
            }


            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return false;
            }

            if (obj.ToString().ToLower() == "true" ||
                obj.ToString().ToLower() == "false" ||
                obj.ToString().ToLower() == "1" ||
                obj.ToString().ToLower() == "0")
            {
                res = obj.ToString().ToLower() == "true" || obj.ToString().ToLower() == "1";
                return true;
            }

            return bool.TryParse(obj.ToString(), out res);
        }

        public static List<string> GetPropertiesName(this object obj)
        {
            var propertyList = new List<string>();
            if (obj == null)
            {
                return propertyList;
            }

            foreach (var prop in obj.GetType().GetProperties())
            {
                propertyList.Add(prop.Name);
            }

            return propertyList;
        }

        public static string GetPropertyName(this object obj, string propertyNameIgnoreCase)
        {
            if (obj == null)
            {
                return null;
            }

            foreach (var prop in obj.GetType().GetProperties())
            {
                if (string.Equals(propertyNameIgnoreCase, prop.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return prop.Name;
                }
            }

            return null;
        }

        public static bool HasProperty(this object obj, string propertyName, bool isIgnoreCase=true)
        {
            if (isIgnoreCase)
            {
                return obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase) != null;
            }

            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static object GetPropertyValue(this object obj, string objectPropertyName)
        {
            if (objectPropertyName == null)
            {
                return null;
            }
            
            var prop = obj.GetType().GetProperty(objectPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanRead)
            {
                return prop.GetValue(obj);
            }

            return null;
        }

        public static Type GetPropertyType(this object obj, string objectPropertyName)
        {
            if (string.IsNullOrEmpty(objectPropertyName))
            {
                return null;
            }

            var prop = obj.GetType().GetProperty(objectPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return prop?.PropertyType;
        }

        public static object GetPropertyNewValue<T>(this object obj)
        {
            if (obj == null)
            {
                return default(T);
            }

            return obj;
        }

        public static object SetPropertyValueByName(this object obj, string propertyName, object value)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                return obj;
            }
            var propertyType = obj.GetPropertyType(propertyName);

            var val = value is DBNull ? null : value;
            if (val == null)
            {
                propertyInfo.SetValue(obj, null, null);
                return obj;
            }

            if (propertyType == typeof(long))
            {
                long.TryParse(val.ToString(), out var parsedValue);
                propertyInfo.SetValue(obj, parsedValue, null);
                return obj;
            }
            else if (propertyType == typeof(int))
            {
                int.TryParse(val.ToString(), out var parsedValue);
                propertyInfo.SetValue(obj, parsedValue, null);
                return obj;
            }
            else if (propertyType == typeof(double))
            {
                double.TryParse(val.ToString(), out var parsedValue);
                propertyInfo.SetValue(obj, parsedValue, null);
                return obj;
            }
            else if (propertyType == typeof(decimal))
            {
                decimal.TryParse(val.ToString(), out var parsedValue);
                propertyInfo.SetValue(obj, parsedValue, null);
                return obj;
            }
            else if (propertyType == typeof(DateTime))
            {
                var parsedValue = Convert.ToDateTime(val.ToString());
                propertyInfo.SetValue(obj, parsedValue, null);
                return obj;
            }
            else if (propertyType == typeof(bool))
            {
                if (bool.TryParse(val.ToString(), out var parsedValue))
                {
                    propertyInfo.SetValue(obj, parsedValue, null);
                    return obj;
                }
                else
                {
                    var parsedValue2 = Convert.ToBoolean(val);
                    propertyInfo.SetValue(obj, parsedValue2, null);
                    return obj;
                }

            }

            propertyInfo.SetValue(obj, val, null);

            return obj;
        }
    }    
}
