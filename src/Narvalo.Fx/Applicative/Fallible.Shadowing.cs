// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public partial struct Fallible<T>
    {
        public Fallible<TResult> Gather<TResult>(Fallible<Func<T, TResult>> applicative)
            => IsSuccess && applicative.IsSuccess
            ? Fallible<TResult>.η(applicative.Value(Value))
            : Fallible<TResult>.FromError(Error);

        public Fallible<TResult> ReplaceBy<TResult>(TResult other)
            => IsSuccess ? Fallible<TResult>.η(other) : Fallible<TResult>.FromError(Error);

        public Fallible<TResult> ContinueWith<TResult>(Fallible<TResult> other)
            => IsSuccess ? other : Fallible<TResult>.FromError(Error);

        public Fallible<T> PassBy<TOther>(Fallible<TOther> other)
            // Returning "this" is not very "functional"-like, but having a value type, that's fine.
            => IsSuccess && other.IsSuccess ? this : FromError(Error);

        public Fallible<Unit> Skip()
            => IsSuccess ? Fallible.Unit : Fallible<Unit>.FromError(Error);

        public Fallible<TResult> ZipWith<TSecond, TResult>(
            Fallible<TSecond> second,
            Func<T, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));
            return IsSuccess && second.IsSuccess
                ? Fallible<TResult>.η(zipper(Value, second.Value))
                : Fallible<TResult>.FromError(Error);
        }

        public Fallible<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));
            return IsSuccess ? Fallible<TResult>.η(selector(Value)) : Fallible<TResult>.FromError(Error);
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
