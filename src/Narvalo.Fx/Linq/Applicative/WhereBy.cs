// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq.Applicative
{
#if EXTENDED_LINQ

    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    public static partial class Kperators
    {
        internal static Fallible<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Fallible<bool>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return Fallible.Of(WhereAnyImpl(source, predicate));
        }

        internal static Maybe<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Maybe<bool>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return Maybe.Of(WhereAnyImpl(source, predicate));
        }

        internal static Outcome<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Outcome<bool>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return Outcome.Of(WhereAnyImpl(source, predicate));
        }
    }

#endif
}
