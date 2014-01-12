﻿namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Diagnostics;

    public static class EnumerableExtensions
    {
        #region > Reduce <

        public static TAccumulate FoldLeft<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Requires.Object(@this);

            return @this.Aggregate(seed, accumulator);

            //TAccumulate acc = seed;

            //foreach (TSource item in @this) {
            //    acc = fun(acc, item);
            //}

            //return acc;
        }

       // foldBack en F# ou foldR en Haskell.
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
