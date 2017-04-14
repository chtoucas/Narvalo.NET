// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public partial struct Result<T, TError>
    {
        public Result<TResult, TError> Gather<TResult>(Result<Func<T, TResult>, TError> applicative)
            => IsError && applicative.IsError
            ? Result<TResult, TError>.FromError(Error)
            : Result<TResult, TError>.Of(applicative.Value(Value));

        public Result<TResult, TError> ReplaceBy<TResult>(TResult value)
            => IsError ? Result<TResult, TError>.FromError(Error) : Result<TResult, TError>.Of(value);

        public Result<TResult, TError> ContinueWith<TResult>(Result<TResult, TError> other)
            => IsError ? Result<TResult, TError>.FromError(Error) : other;

        public Result<T, TError> PassBy<TOther>(Result<TOther, TError> other)
            // Returning "this" is not very "functional"-like, but having a value type, that's fine.
            => IsError && other.IsError ? FromError(Error) : this;

        public Result<Unit, TError> Skip()
            => IsError ? Result<Unit, TError>.FromError(Error) : Result<TError>.Unit;

        public Result<TResult, TError> ZipWith<TSecond, TResult>(
            Result<TSecond, TError> second,
            Func<T, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));
            return IsError && second.IsError
                ? Result<TResult, TError>.FromError(Error)
                : Result<TResult, TError>.Of(zipper(Value, second.Value));
        }

        public Result<TResult, TError> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));
            return IsError
                ? Result<TResult, TError>.FromError(Error)
                : Result<TResult, TError>.Of(selector(Value));
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

