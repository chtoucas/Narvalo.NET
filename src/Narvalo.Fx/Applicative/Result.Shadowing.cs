// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
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
                ? Result<TResult, TError>.Of(selector(Value))
                : Result<TResult, TError>.FromError(Error); ;
        }

        public Result<TResult, TError> ReplaceBy<TResult>(TResult value)
            => IsSuccess ? Result<TResult, TError>.Of(value) : Result<TResult, TError>.FromError(Error);

        public Result<TResult, TError> Then<TResult>(Result<TResult, TError> other)
            => IsSuccess ? other : Result<TResult, TError>.FromError(Error);

        public Result<IEnumerable<T>, TError> Repeat(int count)
            => IsSuccess
            ? Result<IEnumerable<T>, TError>.Of(Enumerable.Repeat(Value, count))
            : Result<IEnumerable<T>, TError>.FromError(Error);
    }

    public partial struct Result<T>
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public Result<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            if (IsError) { Result<TResult>.FromError(ExceptionInfo); }

            try
            {
                return Result.Of(selector.Invoke(Value));
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return Result<TResult>.FromError(edi);
            }
        }

        public Result<TResult> Then<TResult>(Result<TResult> other)
            => IsSuccess ? other : Result<TResult>.FromError(ExceptionInfo);
    }

    public static partial class ResultExtensions
    {
        internal static Result<IEnumerable<TSource>, TError> CollectImpl<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Result<IEnumerable<TSource>, TError>.Of(CollectAnyIterator(@this));
        }

        internal static Result<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Result<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Result.Of(CollectAnyIterator(@this));
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

