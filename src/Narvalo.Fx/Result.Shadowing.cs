// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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

    public static partial class Result
    {
        internal static Result<IEnumerable<TSource>, TError> CollectImpl<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Result.Of<IEnumerable<TSource>, TError>(CollectAnyIterator(@this));
        }
    }
}
