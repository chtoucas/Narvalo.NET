// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public partial class Either<TLeft, TRight>
    {
        public Either<TResult, TRight> Gather<TResult>(Either<Func<TLeft, TResult>, TRight> applicative)
            => IsLeft && applicative.IsLeft
            ? Either<TResult, TRight>.OfLeft(applicative.Left(Left))
            : Either<TResult, TRight>.OfRight(Right);

        public Either<TResult, TRight> ReplaceBy<TResult>(TResult value)
            => IsLeft ? Either<TResult, TRight>.OfLeft(value) : Either<TResult, TRight>.OfRight(Right);

        public Either<TResult, TRight> ContinueWith<TResult>(Either<TResult, TRight> other)
            => IsLeft ? other : Either<TResult, TRight>.OfRight(Right);

        public Either<TLeft, TRight> PassBy<TOther>(Either<TOther, TRight> other)
            // Returning "this" is not very "functional"-like, but having a value type, that's fine.
            => IsLeft && other.IsLeft ? this : OfRight(Right);

        public Either<Unit, TRight> Skip()
            => IsLeft ? Either<Unit, TRight>.OfLeft(Unit.Default) : Either<Unit, TRight>.OfRight(Right);

        public Either<TResult, TRight> ZipWith<TSecond, TResult>(
            Either<TSecond, TRight> second,
            Func<TLeft, TSecond, TResult> zipper)
        {
            Require.NotNull(zipper, nameof(zipper));

            return IsLeft && second.IsLeft
                ? Either<TResult, TRight>.OfLeft(zipper(Left, second.Left))
                : Either<TResult, TRight>.OfRight(Right);
        }

        public Either<TResult, TRight> Select<TResult>(Func<TLeft, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsLeft
                ? Either<TResult, TRight>.OfLeft(selector(Left))
                : Either<TResult, TRight>.OfRight(Right); ;
        }

        public Either<TResult, TRight> SelectMany<TMiddle, TResult>(
            Func<TLeft, Either<TMiddle, TRight>> selector,
            Func<TLeft, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (IsRight) { return Either<TResult, TRight>.OfRight(Right); }
            var middle = selector(Left);

            if (middle.IsRight) { return Either<TResult, TRight>.OfRight(Right); }
            return Either<TResult, TRight>.OfLeft(resultSelector(Left, middle.Left));
        }
    }
}

