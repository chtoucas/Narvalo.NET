// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;
    using System.Linq;

    public partial struct Result<T, TError>
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public Result<TResult, TError> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSuccess
                ? Result.Of<TResult, TError>(selector(Value))
                : Result.FromError<TResult, TError>(Error); ;
        }

        public Result<TResult, TError> ReplaceBy<TResult>(TResult value)
            => IsSuccess ? Result.Of<TResult, TError>(value) : Result.FromError<TResult, TError>(Error);

        public Result<TResult, TError> Then<TResult>(Result<TResult, TError> other)
            => IsSuccess ? other : Result.FromError<TResult, TError>(Error);

        public Result<IEnumerable<T>, TError> Repeat(int count)
            => IsSuccess
            ? Result.Of<IEnumerable<T>, TError>(Enumerable.Repeat(Value, count))
            : Result.FromError<IEnumerable<T>, TError>(Error);
    }

    public partial struct Result<T>
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public Result<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            if (IsError) { Result.FromError<TResult>(ExceptionInfo); }

            try
            {
                return Result.Of(selector.Invoke(Value));
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return Result.FromError<TResult>(edi);
            }
        }

        public Result<TResult> Then<TResult>(Result<TResult> other)
            => IsSuccess ? other : Result.FromError<TResult>(ExceptionInfo);
    }

    public static partial class ResultExtensions
    {
        internal static Result<IEnumerable<TSource>, TError> CollectImpl<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Result.Of<IEnumerable<TSource>, TError>(CollectAnyIterator(@this));
        }

        internal static Result<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Result<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Result.Of(CollectAnyIterator(@this));
        }
    }
}

namespace Narvalo.Fx.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        internal static Result<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Result<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            return Result.Of(WhereAnyIterator(@this, predicate));
        }
    }
}

