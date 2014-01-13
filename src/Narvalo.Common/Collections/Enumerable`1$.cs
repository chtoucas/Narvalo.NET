namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Narvalo.Fx;

    public static class EnumerableExtensions
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

        #region > Single <

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this)
        {
            return Maybe.FirstOrNone(@this, _ => true);
        }

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this, Predicate<T> predicate)
        {
            return Maybe.FirstOrNone(@this, predicate);
        }

        public static Maybe<T> LastOrNone<T>(this IEnumerable<T> @this)
        {
            return Maybe.LastOrNone(@this, _ => true);
        }

        public static Maybe<T> LastOrNone<T>(this IEnumerable<T> @this, Predicate<T> predicate)
        {
            return Maybe.LastOrNone(@this, predicate);
        }

        public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> @this)
        {
            return Maybe.SingleOrNone(@this, _ => true);
        }

        public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> @this, Predicate<T> predicate)
        {
            return Maybe.SingleOrNone(@this, predicate);
        }

        #endregion
    }
}
