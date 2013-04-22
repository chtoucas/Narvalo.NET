namespace Narvalo.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo;

    internal static class EnumerableExtensions
    {
        /// <summary>
        /// An order independent version of <see cref="Enumerable.SequenceEqual{TSource}(System.Collections.Generic.IEnumerable{TSource},System.Collections.Generic.IEnumerable{TSource})"/>.
        /// </summary>
        public static bool SetEqual<T>(this IEnumerable<T> source, IEnumerable<T> y)
        {
            Requires.NotNull(y, "y");
            Requires.Object(source);

            var objectsInX = source.ToList();
            var objectsInY = y.ToList();

            if (objectsInX.Count() != objectsInY.Count()) {
                return false;
            }

            foreach (var objectInY in objectsInY) {
                if (!objectsInX.Contains(objectInY)) {
                    return false;
                }

                objectsInX.Remove(objectInY);
            }

            return !objectsInX.Any();
        }
    }
}