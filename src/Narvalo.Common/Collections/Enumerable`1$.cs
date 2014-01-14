namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static partial class EnumerableExtensions
    {
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
