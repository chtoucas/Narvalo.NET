// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    public partial class ResultOrError<T>
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public abstract ResultOrError<TResult> Select<TResult>(Func<T, TResult> selector);

        public abstract ResultOrError<TResult> ReplaceBy<TResult>(ResultOrError<TResult> other);

        private partial class Success_
        {
            [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
            public override ResultOrError<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                Require.NotNull(selector, nameof(selector));

                try
                {
                    return ResultOrError.Of(selector.Invoke(Value));
                }
                catch (Exception ex)
                {
                    var edi = ExceptionDispatchInfo.Capture(ex);
                    return ResultOrError.FromError<TResult>(edi);
                }
            }

            public override ResultOrError<TResult> ReplaceBy<TResult>(ResultOrError<TResult> other) => other;
        }

        private partial class Error_
        {
            public override ResultOrError<TResult> Select<TResult>(Func<T, TResult> selector)
                => ResultOrError.FromError<TResult>(ExceptionInfo);

            public override ResultOrError<TResult> ReplaceBy<TResult>(ResultOrError<TResult> other)
                => ResultOrError.FromError<TResult>(ExceptionInfo);
        }
    }

    public static partial class ResultOrError
    {
        internal static ResultOrError<IEnumerable<TSource>> CollectImpl<TSource>(this IEnumerable<ResultOrError<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<ResultOrError<IEnumerable<TSource>>>();

            return ResultOrError.Of(CollectAnyIterator(@this));
        }
    }
}

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        internal static ResultOrError<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, ResultOrError<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return ResultOrError.Of(WhereAnyIterator(@this, predicate));
        }
    }
}
