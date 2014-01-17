namespace Narvalo.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Collections.Generic.IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> @this, T element)
        {
            Require.Object(@this);

            return Append_(@this, element);
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> @this, T element)
        {
            Require.Object(@this);

            return Prepend_(@this, element);
        }

        static IEnumerable<T> Append_<T>(IEnumerable<T> source, T element)
        {
            foreach (var item in source) {
                yield return item;
            }

            yield return element;
        }

        static IEnumerable<T> Prepend_<T>(IEnumerable<T> source, T element)
        {
            yield return element;
            foreach (var item in source) {
                yield return item;
            }
        }
    }
}
