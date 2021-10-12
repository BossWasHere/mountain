using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Utils
{
    public static class CollectionUtils
    {
        public static IEnumerable<T> ReadOnlyEnumerable<T>(this ICollection<T> collection)
        {
            foreach (T item in collection)
            {
                yield return item;
            }
        }
    }
}
