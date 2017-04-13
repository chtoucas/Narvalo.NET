// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public partial struct Outcome<T>
    {
        public Outcome<TResult> Gather<TResult>(Outcome<Func<T, TResult>> applicative)
            => IsSuccess && applicative.IsSuccess
            ? Outcome<TResult>.η(applicative.Value(Value))
            : Outcome<TResult>.FromError(Error);

        public Outcome<TResult> ReplaceBy<TResult>(TResult other)
            => IsSuccess ? Outcome<TResult>.η(other) : Outcome<TResult>.FromError(Error);

        public Outcome<TResult> ContinueWith<TResult>(Outcome<TResult> other)
            => IsSuccess ? other : Outcome<TResult>.FromError(Error);

        public Outcome<T> PassBy<TOther>(Outcome<TOther> other)
            // Returning "this" is not very "functional"-like, but having a value type, that's fine.
            => IsSuccess && other.IsSuccess ? this : FromError(Error);

        public Outcome<Unit> Skip()
            => IsSuccess ? Outcome.Unit : Outcome<Unit>.FromError(Error);

        public Outcome<TResult> ZipWith<TSecond, TResult>(
            Outcome<TSecond> second,
            Func<T, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));

            return IsSuccess && second.IsSuccess
                ? Outcome<TResult>.η(zipper(Value, second.Value))
                : Outcome<TResult>.FromError(Error);
        }

        public Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSuccess ? Outcome<TResult>.η(selector(Value)) : Outcome<TResult>.FromError(Error);
        }

        public Outcome<TResult> SelectMany<TMiddle, TResult>(
            Func<T, Outcome<TMiddle>> selector,
            Func<T, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (IsError) { return Outcome<TResult>.FromError(Error); }
            var middle = selector(Value);

            if (middle.IsError) { return Outcome<TResult>.FromError(Error); }
            return Outcome<TResult>.η(resultSelector(Value, middle.Value));
        }
    }
}
