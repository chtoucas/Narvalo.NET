// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, ResultOrError<TResult>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return SelectAnyIterator(@this, selector);
        }

        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, ResultOrError<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            return WhereAnyIterator(@this, predicate);
        }

        private static IEnumerable<TResult> SelectAnyIterator<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, ResultOrError<TResult>> selector)
        {
            Demand.NotNull(source);
            Demand.NotNull(selector);

            foreach (var item in source)
            {
                var m = selector.Invoke(item);

                if (m.IsSuccess) { yield return m.Value; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, ResultOrError<bool>> predicate)
        {
            Demand.NotNull(source);
            Demand.NotNull(predicate);

            foreach (var item in source)
            {
                var m = predicate.Invoke(item);

                if (m.IsSuccess && m.Value) { yield return item; }
            }
        }
    }
}
