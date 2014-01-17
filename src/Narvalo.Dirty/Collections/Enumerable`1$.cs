namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static ICollection<T> ToCollection<T>(this IEnumerable<T> @this)
        {
            Require.Object(@this);

            var result = new Collection<T>();

            foreach (T item in @this) {
                result.Add(item);
            }

            return result;
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

        public static bool IsEmpty<T>(this IEnumerable<T> @this)
        {
            Require.Object(@this);

            return !@this.Any();
        }

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
    }
}
