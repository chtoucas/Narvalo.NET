// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Fx;

    public static partial class EnumerableExtensions
    {
        //// Restriction Operators

        public static IEnumerable<T> Where<T>(
            this IEnumerable<T> @this,
            Func<T, Maybe<bool>> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");

            return from item in @this
                   where predicateM.Invoke(item).ValueOrElse(false)
                   select item;
        }

        //// Conversion Operators

        public static IEnumerable<TResult> ConvertAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> converterM)
        {
            Require.Object(@this);
            Require.NotNull(converterM, "converterM");

            return from item in @this
                   let m = converterM.Invoke(item)
                   where m.IsSome
                   select m.Value;
        }
    }
}
