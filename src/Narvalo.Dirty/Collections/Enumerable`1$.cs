namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> @this)
        {
            Requires.Object(@this);

            return !@this.Any();
        }

        #region > Aggregate <

        public static TAccumulate FoldLeft<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Requires.Object(@this);

            return @this.Aggregate(seed, accumulator);
        }

        public static TAccumulate FoldRight<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Requires.Object(@this);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static T Reduce<T>(
            this IEnumerable<T> @this,
            Func<T, T, T> accumulator)
        {
            Requires.Object(@this);

            return @this.Aggregate(accumulator);
        }

        #endregion
    }
}
