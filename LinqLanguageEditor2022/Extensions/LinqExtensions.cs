using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqLanguageEditor2022.Extensions
{
    public static class LinqExtensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return s == null || s.Trim().Length == 0;
        }
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return !(list?.Any() ?? false);
        }
        public static bool NotNullAny<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable?.Any(predicate) == true;
        }
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data?.Any() == true;
        }

    }
}
