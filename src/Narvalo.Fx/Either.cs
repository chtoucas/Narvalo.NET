// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    public static partial class Either
    {
        public static Either<TLeft, TRight> FromLeft<TLeft, TRight>(TLeft value)
        {
            Warrant.NotNull<Either<TLeft, TRight>>();

            return Either<TLeft, TRight>.η(value);
        }

        public static Either<TLeft, TRight> FromRight<TLeft, TRight>(TRight value)
        {
            Warrant.NotNull<Either<TLeft, TRight>>();

            return Either<TLeft, TRight>.η(value);
        }

        public static Either<TLeft, TRight> Flatten<TLeft, TRight>(Either<Either<TLeft, TRight>, TRight> square)
        {
            Expect.NotNull(square);

            return Either<TLeft, TRight>.μ(square);
        }

        public static Either<TLeft, TRight> Flatten<TLeft, TRight>(Either<TLeft, Either<TLeft, TRight>> square)
        {
            Expect.NotNull(square);

            return Either<TLeft, TRight>.μ(square);
        }
    }

    // Provides extension methods for Either<TLeft, TRight>.
    public static partial class Either
    {
        public static Either<TResult, TRight> SelectLeft<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TLeft, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return @this.BindLeft(_ => FromLeft<TResult, TRight>(selector.Invoke(_)));
        }

        public static Either<TLeft, TResult> SelectRight<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TRight, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return @this.BindRight(_ => FromRight<TLeft, TResult>(selector.Invoke(_)));
        }
    }
}
