// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions { }

    // Provides extension methods w/ Maybe.
    public static partial class EnumerableExtensions
    {
        // Named <c>mapMaybe</c> in Haskell parlance.
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> selectorM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selectorM, nameof(selectorM));
            Warrant.NotNull<IEnumerable<TResult>>();

            return SelectAnyIterator(@this, selectorM);
        }

        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicateM, nameof(predicateM));
            Warrant.NotNull<IEnumerable<TSource>>();

            return WhereAnyIterator(@this, predicateM);
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> selectorM)
        {
            Demand.NotNull(source);
            Demand.NotNull(selectorM);
            Warrant.NotNull<IEnumerable<TResult>>();

            foreach (var item in source)
            {
                var m = selectorM.Invoke(item);

                if (m.IsSome) { yield return m.Value; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Demand.NotNull(source);
            Demand.NotNull(predicateM);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                var flg = predicateM.Invoke(item);

                if (flg.IsSome && flg.Value)
                {
                    yield return item;
                }
            }
        }
    }

    // Provides extension methods w/ Outcome.
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<TResult>> selectorM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selectorM, nameof(selectorM));
            Warrant.NotNull<IEnumerable<TResult>>();

            return SelectAnyIterator(@this, selectorM);
        }

        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<bool>> predicateM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicateM, nameof(predicateM));
            Warrant.NotNull<IEnumerable<TSource>>();

            return WhereAnyIterator(@this, predicateM);
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Outcome<TResult>> selectorM)
        {
            Demand.NotNull(source);
            Demand.NotNull(selectorM);
            Warrant.NotNull<IEnumerable<TResult>>();

            foreach (var item in source)
            {
                var m = selectorM.Invoke(item);

                if (m.IsSuccess) { yield return m.Value; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Outcome<bool>> predicateM)
        {
            Demand.NotNull(source);
            Demand.NotNull(predicateM);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                var flg = predicateM.Invoke(item);

                if (flg.IsSuccess && flg.Value)
                {
                    yield return item;
                }
            }
        }
    }
}
