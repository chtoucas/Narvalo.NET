namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Narvalo.Fx;

    public static class EnumerableExtensions
    {
        #region > Conversions <

        public static ICollection<T> ToCollection<T>(this IEnumerable<T> source)
        {
            Requires.NotNull(source);

            var result = new Collection<T>();

            foreach (T item in source) {
                result.Add(item);
            }

            return result;
        }

        public static IList<T> ToList<T>(this IEnumerable<T> source)
        {
            Requires.NotNull(source);

            var result = new List<T>();

            foreach (T item in source) {
                result.Add(item);
            }

            return result;
        }

        #endregion

        #region > Single <

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> source)
        {
            return Maybe.FirstOrNone(source, _ => true);
        }

        public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return Maybe.FirstOrNone(source, predicate);
        }

        public static Maybe<T> LastOrNone<T>(this IEnumerable<T> source)
        {
            return Maybe.LastOrNone(source, _ => true);
        }

        public static Maybe<T> LastOrNone<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return Maybe.LastOrNone(source, predicate);
        }

        public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> source)
        {
            return Maybe.SingleOrNone(source, _ => true);
        }

        public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return Maybe.SingleOrNone(source, predicate);
        }

        #endregion
    }
}
