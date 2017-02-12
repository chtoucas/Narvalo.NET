// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    // Overrides a bunch of auto-generated (extension) methods.
    public abstract partial class Outcome<T>
    {
        public Outcome<TResult> Then<TResult>(Outcome<TResult> other)
            => IsSuccess ? other : Outcome<TResult>.η(ExceptionInfo);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public abstract Outcome<TResult> Select<TResult>(Func<T, TResult> selector);

        private sealed partial class Success_
        {
            public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, nameof(selector));

                return Outcome<TResult>.η(selector.Invoke(Value));
            }
        }

        private sealed partial class Error_
        {
            public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
                => Outcome<TResult>.η(ExceptionInfo);
        }
    }

    // Overrides for auto-generated (extension) methods on IEnumerable<Outcome<T>>.
    public static partial class Sequence
    {
        internal static Outcome<IEnumerable<TSource>> CollectImpl<TSource>(this IEnumerable<Outcome<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Outcome<IEnumerable<TSource>>>();

            return Outcome.Success(CollectAnyIterator(@this));
        }
    }
}

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    // Overrides for auto-generated (extension) methods on IEnumerable<T>.
    public static partial class MoreEnumerable
    {
        internal static Outcome<IEnumerable<TSource>> FilterImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return Outcome.Success(WhereAnyIterator(@this, predicate));
        }
    }
}
