using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class LinqHelpers
    {
        /// <summary>
        /// Takes an enumerable of enumerables of type T and concatenates
        /// the inner enumerables into a flat list.
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) {
            return source.SelectMany(x => x);
        }
    }
}
