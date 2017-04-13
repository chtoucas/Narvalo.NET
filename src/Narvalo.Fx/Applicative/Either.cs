// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System.Diagnostics.CodeAnalysis;

    public static partial class Either
    {
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "[Intentionally] Fluent API.")]
        public static class OfTRight<TRight>
        {
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
            public static Either<TLeft, TRight> OfLeft<TLeft>(TLeft leftValue)
                => Either<TLeft, TRight>.OfLeft(leftValue);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "[Intentionally] Fluent API.")]
        public static class OfTLeft<TLeft>
        {
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
            public static Either<TLeft, TRight> OfRight<TRight>(TRight rightValue)
                => Either<TLeft, TRight>.OfRight(rightValue);
        }
    }

    public static partial class EitherL
    {
        public static Either<TLeft, TRight> Flatten<TLeft, TRight>(
            this Either<TLeft, Either<TLeft, TRight>> square)
            => Either<TLeft, TRight>.μ(square);
    }
}
