// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    public static class Either
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
    }
}
