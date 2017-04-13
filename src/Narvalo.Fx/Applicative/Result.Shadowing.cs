// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public partial struct Result<T, TError>
    {
        public Result<TResult, TError> Gather<TResult>(Result<Func<T, TResult>, TError> applicative)
            => IsSuccess && applicative.IsSuccess
            ? Result<TResult, TError>.Of(applicative.Value(Value))
            : Result<TResult, TError>.FromError(Error);

        public Result<TResult, TError> ReplaceBy<TResult>(TResult value)
            => IsSuccess ? Result<TResult, TError>.Of(value) : Result<TResult, TError>.FromError(Error);

        public Result<TResult, TError> ContinueWith<TResult>(Result<TResult, TError> other)
            => IsSuccess ? other : Result<TResult, TError>.FromError(Error);

        public Result<T, TError> PassBy<TOther>(Result<TOther, TError> other)
            // Returning "this" is not very "functional"-like, but having a value type, that's fine.
            => IsSuccess && other.IsSuccess ? this : FromError(Error);

        public Result<Unit, TError> Skip()
            => IsSuccess ? Result<Unit, TError>.Of(Unit.Default) : Result<Unit, TError>.FromError(Error);

        public Result<TResult, TError> ZipWith<TSecond, TResult>(
            Result<TSecond, TError> second,
            Func<T, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));
            return IsSuccess && second.IsSuccess
                ? Result<TResult, TError>.Of(zipper(Value, second.Value))
                : Result<TResult, TError>.FromError(Error);
        }

        public Result<TResult, TError> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));
            return IsSuccess
                ? Result<TResult, TError>.Of(selector(Value))
                : Result<TResult, TError>.FromError(Error); ;
        }

        public Result<TResult, TError> SelectMany<TMiddle, TResult>(
            Func<T, Result<TMiddle, TError>> selector,
            Func<T, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (IsError) { return Result<TResult, TError>.FromError(Error); }
            var middle = selector(Value);

            if (middle.IsError) { return Result<TResult, TError>.FromError(Error); }
            return Result<TResult, TError>.Of(resultSelector(Value, middle.Value));
        }
    }
}

