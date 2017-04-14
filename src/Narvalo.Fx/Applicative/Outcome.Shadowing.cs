// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public partial struct Outcome<T>
    {
        public Outcome<TResult> Gather<TResult>(Outcome<Func<T, TResult>> applicative)
            => IsError && applicative.IsError
            ? Outcome<TResult>.FromError(Error)
            : Outcome<TResult>.η(applicative.Value(Value));

        public Outcome<TResult> ReplaceBy<TResult>(TResult other)
            => IsError ? Outcome<TResult>.FromError(Error) : Outcome<TResult>.η(other);

        public Outcome<TResult> ContinueWith<TResult>(Outcome<TResult> other)
            => IsError ? Outcome<TResult>.FromError(Error) : other;

        public Outcome<T> PassBy<TOther>(Outcome<TOther> other)
            // Returning "this" is not very "functional"-like, but having a value type, that's fine.
            => IsError && other.IsError ? FromError(Error) : this;

        public Outcome<Unit> Skip()
            => IsError ? Outcome<Unit>.FromError(Error) : Outcome.Unit;

        public Outcome<TResult> ZipWith<TSecond, TResult>(
            Outcome<TSecond> second,
            Func<T, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));
            return IsError && second.IsError
                ? Outcome<TResult>.FromError(Error)
                : Outcome<TResult>.η(zipper(Value, second.Value));
        }

        public Outcome<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));
            return IsError ? Outcome<TResult>.FromError(Error) : Outcome<TResult>.η(selector(Value));
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
