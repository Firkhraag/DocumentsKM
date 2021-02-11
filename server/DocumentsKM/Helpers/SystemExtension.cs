using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DocumentsKM
{
    public static class SystemExtension
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(
            this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(
                input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static bool Validate(this object input)
        {
            foreach(PropertyInfo pi in input.GetType().GetProperties())
            {
                var value = pi.GetValue(input);
                if (value != null)
                    return true;
            }
            return false;
        }
    }
}
