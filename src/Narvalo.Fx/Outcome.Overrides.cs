// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;

    // Overrides a bunch of auto-generated (extension) methods.
    public abstract partial class Outcome<T>
    {
        public Outcome<TResult> Then<TResult>(Outcome<TResult> other)
            => IsSuccess ? other : Outcome<TResult>.η(ExceptionInfo);
    }

    // Overrides for auto-generated (extension) methods on IEnumerable<Outcome<T>>.
    public static partial class EnumerableExtensions
    {
        internal static Outcome<IEnumerable<TSource>> CollectImpl<TSource>(this IEnumerable<Outcome<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Outcome<IEnumerable<TSource>>>();

            return Outcome.Success(CollectAnyIterator(@this));
        }
    }
}

namespace Narvalo.Fx.More
{
    using System;
    using System.Collections.Generic;

    // Overrides for auto-generated (extension) methods on IEnumerable<T>.
    public static partial class EnumerableExtensions
    {
        internal static Outcome<IEnumerable<TSource>> FilterImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<bool>> predicateM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicateM, nameof(predicateM));
            Warrant.NotNull<IEnumerable<TSource>>();

            return Outcome.Success(WhereAnyIterator(@this, predicateM));
        }
    }
}
