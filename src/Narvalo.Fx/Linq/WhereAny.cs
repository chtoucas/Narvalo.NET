// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool?> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return WhereAnyIterator(@this, predicate);
        }

        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return WhereAnyIterator(@this, predicate);
        }

        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return WhereAnyIterator(@this, predicate);
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, bool?> predicate)
        {
            Demand.NotNull(source);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                var m = predicate.Invoke(item);

                if (m.HasValue && m.Value) { yield return item; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<bool>> predicate)
        {
            Demand.NotNull(source);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                var m = predicate.Invoke(item);

                if (m.IsSome && m.Value) { yield return item; }
            }
        }

        private static IEnumerable<TSource> WhereAnyIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Outcome<bool>> predicate)
        {
            Demand.NotNull(source);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                var m = predicate.Invoke(item);

                if (m.IsSuccess && m.Value) { yield return item; }
            }
        }
    }
}
