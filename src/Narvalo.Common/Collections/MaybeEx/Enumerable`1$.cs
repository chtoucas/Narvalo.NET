// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections.MaybeEx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Fx;

    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> funM)
        {
            Require.NotNull(funM, "funM");

            return from _ in @this
                   let m = funM.Invoke(_)
                   where m.IsSome
                   select m.Value;
        }

        internal static IEnumerable<TSource> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.NotNull(predicateM, "predicateM");

            return from _ in @this
                   where predicateM.Invoke(_).ValueOrElse(false)
                   select _;
        }
    }
}
