// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;

    public partial struct Outcome<T>
    {
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
            this IEnumerable<Outcome<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Outcome.Of(CollectAnyIterator(@this));
        }
    }
}

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    public static partial class Qperators
    {
        internal static Outcome<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            return Outcome.Of(WhereAnyIterator(@this, predicate));
        }
    }
}

