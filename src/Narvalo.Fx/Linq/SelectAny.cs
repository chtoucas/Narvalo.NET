// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Operators
    {
        // Named <c>mapMaybe</c> in Haskell parlance.
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            Warrant.NotNull<IEnumerable<TResult>>();

            return SelectAnyIterator(@this, selector);
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<TResult>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            Warrant.NotNull<IEnumerable<TResult>>();

            return SelectAnyIterator(@this, selector);
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> selector)
        {
            Demand.NotNull(source);
            Demand.NotNull(selector);
            Warrant.NotNull<IEnumerable<TResult>>();

            foreach (var item in source)
            {
                var m = selector.Invoke(item);

                if (m.IsSome) { yield return m.Value; }
            }
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Outcome<TResult>> selector)
        {
            Demand.NotNull(source);
            Demand.NotNull(selector);
            Warrant.NotNull<IEnumerable<TResult>>();

            foreach (var item in source)
            {
                var m = selector.Invoke(item);

                if (m.IsSuccess) { yield return m.Value; }
            }
        }
    }
}
