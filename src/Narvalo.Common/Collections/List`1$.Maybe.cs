// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Fx;

    public static partial class ListExtensions
    {
        public static Maybe<IList<TResult>> MayConvertAll<TSource, TResult>(
            this IList<TSource> @this,
            Func<TSource, Maybe<TResult>> converterM)
        {
            Require.Object(@this);
            Require.NotNull(converterM, "converterM");

            var seq = from value in @this select converterM(value);

            return seq.ToList().MayPullback();
        }

        public static Maybe<IList<TSource>> MayPullback<TSource>(
            this IList<Maybe<TSource>> @this)
        {
            Require.Object(@this);

            IList<TSource> list = new List<TSource>();

            foreach (var m in @this) {
                if (!m.IsSome) {
                    return Maybe<IList<TSource>>.None;
                }

                list.Add(m.Value);
            }

            return Maybe.Create(list);
        }
    }
}
