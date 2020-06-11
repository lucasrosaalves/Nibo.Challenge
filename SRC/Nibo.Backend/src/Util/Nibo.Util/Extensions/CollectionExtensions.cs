using System.Collections.Generic;
using System.Linq;

namespace Nibo.Util.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @collection)
            => @collection is null || !collection.Any();
    }
}
