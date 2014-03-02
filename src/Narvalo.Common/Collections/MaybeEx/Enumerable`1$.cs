// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections.MaybeEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Fx;

    public static partial class EnumerableExtensions
    {
        public static Maybe<IEnumerable<TSource>> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");

            var seq = from item in @this
                      where predicateM.Invoke(item).ValueOrElse(false)
                      select item;

            return Maybe.Create(seq);
        }

        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");

            return from item in @this
                   let m = funM.Invoke(item)
                   where m.IsSome
                   select m.Value;
        }
    }
}
