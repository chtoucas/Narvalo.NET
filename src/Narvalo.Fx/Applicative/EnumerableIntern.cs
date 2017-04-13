// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using QImpl = Narvalo.Linq.Applicative.EnumerableIntern;

    // **WARNING:** Keep this class internal. It provides LINQ operators,
    // but none of them are composable, therefore it seems like a bad idea to expose them.
    // Moreover they do not perform any arg-check.
    internal static partial class EnumerableIntern { }

    // Shadows for CollectImpl().
    internal static partial class EnumerableIntern
    {
        public static Either<IEnumerable<TSource>, TRight> CollectImpl<TSource, TRight>(
            this IEnumerable<Either<TSource, TRight>> source)
        {
            Debug.Assert(source != null);
            return Either.OfRight<TRight>.OfLeft(QImpl.CollectAnyImpl(source));
        }

        public static Fallible<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Fallible<TSource>> source)
        {
            Debug.Assert(source != null);
            return Fallible.Of(QImpl.CollectAnyImpl(source));
        }

        public static Maybe<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Maybe<TSource>> source)
        {
            Debug.Assert(source != null);
            return Maybe.Of(QImpl.CollectAnyImpl(source));
        }

        public static Outcome<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Outcome<TSource>> source)
        {
            Debug.Assert(source != null);
            return Outcome.Of(QImpl.CollectAnyImpl(source));
        }

        public static Result<IEnumerable<TSource>, TError> CollectImpl<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> source)
        {
            Debug.Assert(source != null);
            return Result.OfError<TError>.Return(QImpl.CollectAnyImpl(source));
        }
    }

    // Shadows for WhereImpl().
    internal static partial class EnumerableIntern
    {
        public static Either<IEnumerable<TSource>, TRight> WhereImpl<TSource, TRight>(
            this IEnumerable<TSource> source,
            Func<TSource, Either<bool, TRight>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);
            return Either.OfRight<TRight>.OfLeft(QImpl.WhereAnyImpl(source, predicate));
        }

        public static Fallible<IEnumerable<TSource>> WhereImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Fallible<bool>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);
            return Fallible.Of(QImpl.WhereAnyImpl(source, predicate));
        }

        public static Maybe<IEnumerable<TSource>> WhereImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Maybe<bool>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);
            return Maybe.Of(QImpl.WhereAnyImpl(source, predicate));
        }

        public static Outcome<IEnumerable<TSource>> WhereImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Outcome<bool>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);
            return Outcome.Of(QImpl.WhereAnyImpl(source, predicate));
        }

        public static Result<IEnumerable<TSource>, TError> WhereImpl<TSource, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<bool, TError>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);
            return Result.OfError<TError>.Return(QImpl.WhereAnyImpl(source, predicate));
        }
    }
}
