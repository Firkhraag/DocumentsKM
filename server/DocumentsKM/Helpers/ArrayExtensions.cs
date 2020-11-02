using System.Collections.Generic;
using System.Linq;

namespace DocumentsKM
{
    public static class ArrayExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}
