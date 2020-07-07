using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class Extensions
    {
        public static bool IsContainedIn<T>(this T t, IEnumerable<T> enumerable) {
            return enumerable.Contains(t);
        }
    }
}