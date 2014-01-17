namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> @this, T element)
        {
            Requires.Object(@this);

            return Append_(@this, element);
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> @this, T element)
        {
            Requires.Object(@this);

            return Prepend_(@this, element);
        }

        static IEnumerable<T> Prepend_<T>(IEnumerable<T> source, T element)
        {
            yield return element;
            foreach (var item in source) {
                yield return item;
            }
        }

        static IEnumerable<T> Append_<T>(IEnumerable<T> source, T element)
        {
            foreach (var item in source) {
                yield return item;
            }
            yield return element;
        }

        #region > Conversions <

        public static ICollection<T> ToCollection<T>(this IEnumerable<T> @this)
        {
            Requires.Object(@this);

            var result = new Collection<T>();

            foreach (T item in @this) {
                result.Add(item);
            }

            return result;
        }

        // Une méthode semblable est fournie par System.Linq.
        //public static IList<T> ToList<T>(this IEnumerable<T> @this)
        //{
        //    Requires.Object(@this);

        //    var result = new List<T>();

        //    foreach (T item in @this) {
        //        result.Add(item);
        //    }

        //    return result;
        //}

        #endregion
    }
}
