// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static partial class Either
    {
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "[Intentionally] Fluent API.")]
        public static class Left<TRight>
        {
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
            public static Either<TLeft, TRight> Return<TLeft>(TLeft leftValue)
                => Either<TLeft, TRight>.OfLeft(leftValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "[Intentionally] Fluent API.")]
        public static class Right<TLeft>
        {
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
            public static Either<TLeft, TRight> Return<TRight>(TRight rightValue)
                => Either<TLeft, TRight>.OfRight(rightValue);
        }
    }

    public static partial class Either
    {
        public static Either<TLeft, TRight> FlattenLeft<TLeft, TRight>(Either<Either<TLeft, TRight>, TRight> square)
            => Either<TLeft, TRight>.μ(square);

        public static Either<TLeft, TRight> FlattenRight<TLeft, TRight>(Either<TLeft, Either<TLeft, TRight>> square)
            => Either<TLeft, TRight>.FlattenRight(square);
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

            return @this.BindLeft(val => Left<TRight>.Return(selector(val)));
        }

        public static Either<TLeft, TResult> SelectRight<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TRight, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return @this.BindRight(val => Right<TLeft>.Return(selector(val)));
        }
    }
}
