// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class MoreEnumerable
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

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<TResult>> selectorM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selectorM, nameof(selectorM));
            Warrant.NotNull<IEnumerable<TResult>>();

            return SelectAnyIterator(@this, selectorM);
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
    }
}
