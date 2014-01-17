namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Fx;

    public static partial class EnumerableExtensions
    {
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
    }
}
