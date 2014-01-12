namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Diagnostics;

    public static class EnumerableExtensions
    {
        #region > Reduce <

        public static TAccumulate FoldLeft<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Requires.NotNull(source, "source");

            return source.Aggregate(seed, accumulator);

            //TAccumulate acc = seed;

            //foreach (TSource item in source) {
            //    acc = fun(acc, item);
            //}

            //return acc;
        }

       // foldBack en F# ou foldR en Haskell.
        public static TAccumulate FoldRight<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Requires.NotNull(source, "source");

            return source.Reverse().Aggregate(seed, accumulator);
        }

        public static T Reduce<T>(
            this IEnumerable<T> source,
            Func<T, T, T> accumulator)
        {
            Requires.NotNull(source, "source");

            return source.Aggregate(accumulator);
        }

        #endregion
    }
}
