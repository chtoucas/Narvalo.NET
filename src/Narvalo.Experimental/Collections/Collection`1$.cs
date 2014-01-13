namespace Narvalo.Internal
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> items)
        {
            Requires.NotNull(target, "target");
            Requires.NotNull(items, "items");

            foreach (var item in items) {
                target.Add(item);
            }
        }
    }
}