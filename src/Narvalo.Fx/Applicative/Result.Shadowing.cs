// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public partial struct Result<T, TError>
    {
        public Result<TResult, TError> ReplaceBy<TResult>(TResult value)
            => IsSuccess ? Result<TResult, TError>.Of(value) : Result<TResult, TError>.FromError(Error);

        public Result<TResult, TError> ContinueWith<TResult>(Result<TResult, TError> other)
            => IsSuccess ? other : Result<TResult, TError>.FromError(Error);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public Result<TResult, TError> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSuccess
                ? Result<TResult, TError>.Of(selector(Value))
                : Result<TResult, TError>.FromError(Error); ;
        }
    }

    public static partial class Result
    {
        internal static Result<IEnumerable<TSource>, TError> CollectImpl<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> source)
        {
            Require.NotNull(source, nameof(source));

            return Result<IEnumerable<TSource>, TError>.Of(CollectAnyIterator(source));
        }
    }
}

