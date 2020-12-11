using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ShadyNagy.Utilities.Extensions.Object;

namespace ShadyNagy.Utilities.Extensions.String
{
    public static class StringExtension
    {
        public static string TrimAndReduce(this string str)
        {
            return ConvertWhitespacesToSingleSpaces(str).Trim();
        }

        public static string ConvertWhitespacesToSingleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        public static string NullToEmpty(this string value)
        {
            return value ?? "";
        }


        public static bool ParseBool(this string str)
        {
            return str.ToLower() == "true";
        }


        public static int ToInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            Int32.TryParse(str, out var res);

            return res;
        }


        public static bool ToBool(this string str)
        {
            var res = false;

            if (string.IsNullOrEmpty(str))
                return false;

            if (str.ToLower() == "true" ||
                str.ToLower() == "false" ||
                str.ToLower() == "1" ||
                str.ToLower() == "0")
            {
                res = str.ToLower() == "true" || str.ToLower() == "1";
                return res;
            }
            bool.TryParse(str, out res);

            return res;
        }

        public static T ToEnum<T>(this string str, T defaultValue) where T : struct
        {
            var res = defaultValue;

            if (string.IsNullOrEmpty(str))
                return defaultValue;


            res = defaultValue;
            if (Enum.TryParse<T>(str, out res))
                return res;

            if (Enum.TryParse<T>(str.Replace(" ", "").ToLower(), out res))
                return res;

            if (Enum.TryParse<T>(str.Replace(" ", "").ToUpper(), out res))
                return res;

            return res;
        }

        public static string RemoveFromString(this string str, IReadOnlyList<string> toRemoveList)
        {
            var res = str;

            foreach (var toRemove in toRemoveList)
            {
                res = str.Replace(toRemove, "");
            }

            return res;
        }

        public static bool ParseBool(this string str, out bool res)
        {
            res = false;

            if (string.IsNullOrEmpty(str))
                return false;

            if (str.ToLower() == "true" ||
                str.ToLower() == "false" ||
                str.ToLower() == "1" ||
                str.ToLower() == "0")
            {
                res = str.ToLower() == "true" || str.ToLower() == "1";
                return true;
            }

            return bool.TryParse(str, out res);
        }

        public static Guid ToGuid(this string str)
        {
            Guid.TryParse(str, out var result);

            return result;
        }

        public static Guid? ToNullableGuid(this string str)
        {
            if (!Guid.TryParse(str, out var result))
            {
                return null;
            }

            return result;
        }

        public static object ToNullableByType(this string str, string outType, string format=null)
        {
        
            if (outType.ToLower() == typeof(string).ToString().ToLower())
                return str;
            else if (outType.ToLower() == typeof(int).ToString().ToLower())
            {
                return str.ConvertToNullableInt();
            }
            else if (outType.ToLower() == typeof(long).ToString().ToLower())
            {
                return str.ConvertToNullableLong();
            }
            else if (outType.ToLower() == typeof(decimal).ToString().ToLower())
            {
                return str.ConvertToNullableDecimal();
            }
            else if (outType.ToLower() == typeof(int?).ToString().ToLower())
            {
                return str.ConvertToNullableInt();
            }
            else if (outType.ToLower() == typeof(long?).ToString().ToLower())
            {
                return str.ConvertToNullableLong();
            }
            else if (outType.ToLower() == typeof(double).ToString().ToLower())
            {
                return str.ConvertToNullableDouble();
            }
            else if (outType.ToLower() == typeof(double?).ToString().ToLower())
            {
                return str.ConvertToNullableDouble();
            }
            else if (outType.ToLower() == typeof(bool).ToString().ToLower())
            {
                return str.ConvertToNullableBool();
            }
            else if (outType.ToLower() == typeof(bool?).ToString().ToLower())
            {
                return str.ConvertToNullableBool();
            }
            else if (outType.ToLower() == typeof(DateTime).ToString().ToLower())
            {
                return str.ConvertToNullableDateTime(format);
            }
            else if (outType.ToLower() == typeof(DateTime?).ToString().ToLower())
            {
                return str.ConvertToNullableDateTime(format);
            }
            else if (outType.ToLower() == typeof(Guid).ToString().ToLower())
            {
                return str.ConvertToNullableGuid();
            }
            else if (outType.ToLower() == typeof(Guid?).ToString().ToLower())
            {
                return str.ConvertToNullableGuid();
            }

            return null;
        }

        private static string SplitByUpperCase(this string data)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);


            return r.Replace(data, " ");
        }
    }    
}
