namespace Narvalo.Collections
{
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        // NB: Une méthode semblable est déjà fournie par System.Linq.
        public static IList<T> ToList<T>(this IEnumerable<T> @this)
        {
            Require.Object(@this);

            var result = new List<T>();

            foreach (T item in @this) {
                result.Add(item);
            }

            return result;
        }
    }
}
