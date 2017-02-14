// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    public partial class Outcome<T>
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public abstract Outcome<TResult> Select<TResult>(Func<T, TResult> selector);

        public abstract Outcome<TResult> Next<TResult>(Outcome<TResult> other);

        private partial class Success_
        {
            [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Exceptionally, we do want to catch all exceptions.")]
            public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, nameof(selector));

                // Catching all exceptions is not a good practice, but here it makes sense, since
                // the type is supposed to encode the exception too.
                try
                {
                    return Outcome.Of(selector.Invoke(Value));
                }
                catch (Exception ex)
                {
                    var edi = ExceptionDispatchInfo.Capture(ex);
                    return Outcome.FromError<TResult>(edi);
                }
            }

            public override Outcome<TResult> Next<TResult>(Outcome<TResult> other) => other;
        }

        private partial class Error_
        {
            public override Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
                => Outcome.FromError<TResult>(ExceptionInfo);

            public override Outcome<TResult> Next<TResult>(Outcome<TResult> other)
                => Outcome.FromError<TResult>(ExceptionInfo);
        }
    }

    public static partial class Sequence
    {
        internal static Outcome<IEnumerable<TSource>> CollectImpl<TSource>(this IEnumerable<Outcome<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Outcome<IEnumerable<TSource>>>();

            return Outcome.Of(CollectAnyIterator(@this));
        }
    }
}

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Operators
    {
        internal static Outcome<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return Outcome.Of(WhereAnyIterator(@this, predicate));
        }
    }
}
