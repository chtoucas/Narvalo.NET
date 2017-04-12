// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial struct Result<T, TError>
    {
        public Result<TResult, TError> ReplaceBy<TResult>(TResult value)
            => IsSuccess ? Result.Ok<TError>.Return(value) : Result.Error<TResult>.Return(Error);

        public Result<TResult, TError> ContinueWith<TResult>(Result<TResult, TError> other)
            => IsSuccess ? other : Result.Error<TResult>.Return(Error);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "[Intentionally] No trouble here, this 'Select' is the one from the LINQ standard query operators.")]
        public Result<TResult, TError> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSuccess
                ? Result.Ok<TError>.Return(selector(Value))
                : Result.Error<TResult>.Return(Error); ;
        }
    }
}

