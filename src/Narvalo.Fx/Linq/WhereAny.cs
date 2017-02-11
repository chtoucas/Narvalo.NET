// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class MoreEnumerable
    {
        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicateM, nameof(predicateM));
            Warrant.NotNull<IEnumerable<TSource>>();

            return WhereAnyIterator(@this, predicateM);
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
