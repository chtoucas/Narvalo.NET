// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;

    public partial struct Outcome<T>
    {
        public Outcome<TResult> Gather<TResult>(Outcome<Func<T, TResult>> applicative)
            => IsSuccess && applicative.IsSuccess
            ? Outcome<TResult>.η(applicative.Value(Value))
            : Outcome<TResult>.FromError(Error);

        public Outcome<TResult> ReplaceBy<TResult>(TResult other)
            => IsSuccess ? Outcome.Of(other) : Outcome<TResult>.FromError(Error);

        public Outcome<TResult> ContinueWith<TResult>(Outcome<TResult> other)
            => IsSuccess ? other : Outcome<TResult>.FromError(Error);

        public Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSuccess ? Outcome.Of(selector(Value)) : Outcome<TResult>.FromError(Error);
        }
    }

    public static partial class OutcomeExtensions
    {
        internal static Outcome<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Outcome<TSource>> source)
        {
            Require.NotNull(source, nameof(source));

            return Outcome.Of(CollectAnyIterator(source));
        }
    }
}

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    public static partial class Sequence
    {
        internal static Outcome<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Outcome<bool>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return Outcome.Of(WhereAnyIterator(source, predicate));
        }
    }
}

