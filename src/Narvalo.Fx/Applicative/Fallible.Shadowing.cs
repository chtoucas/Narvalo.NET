// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public partial struct Fallible<T>
    {
        public Fallible<TResult> Gather<TResult>(Fallible<Func<T, TResult>> applicative)
            => IsError && applicative.IsError
            ? Fallible<TResult>.FromError(Error)
            : Fallible<TResult>.η(applicative.Value(Value));

        public Fallible<TResult> ReplaceBy<TResult>(TResult other)
            => IsError ? Fallible<TResult>.FromError(Error) : Fallible<TResult>.η(other);

        public Fallible<TResult> ContinueWith<TResult>(Fallible<TResult> other)
            => IsError ? Fallible<TResult>.FromError(Error) : other;

        public Fallible<T> PassBy<TOther>(Fallible<TOther> other)
            // Returning "this" is not very "functional"-like, but having a value type, that's fine.
            => IsError && other.IsError ? FromError(Error) : this;

        public Fallible<Unit> Skip()
            => IsError ? Fallible<Unit>.FromError(Error) : Fallible.Unit;

        public Fallible<TResult> ZipWith<TSecond, TResult>(
            Fallible<TSecond> second,
            Func<T, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));
            return IsError && second.IsError
                ? Fallible<TResult>.FromError(Error)
                : Fallible<TResult>.η(zipper(Value, second.Value));
        }

        public Fallible<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));
            return IsError ? Fallible<TResult>.FromError(Error) : Fallible<TResult>.η(selector(Value));
        }

        public Fallible<TResult> SelectMany<TMiddle, TResult>(
            Func<T, Fallible<TMiddle>> selector,
            Func<T, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (IsError) { return Fallible<TResult>.FromError(Error); }
            var middle = selector(Value);

            if (middle.IsError) { return Fallible<TResult>.FromError(Error); }
            return Fallible<TResult>.η(resultSelector(Value, middle.Value));
        }
    }
}
