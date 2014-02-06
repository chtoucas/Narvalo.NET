namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static TAccumulate FoldLeft<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Require.Object(@this);

            return @this.Aggregate(seed, accumulator);
        }

        public static TAccumulate FoldRight<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Require.Object(@this);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static T Reduce<T>(
            this IEnumerable<T> @this,
            Func<T, T, T> accumulator)
        {
            Require.Object(@this);

            return @this.Aggregate(accumulator);
        }

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
