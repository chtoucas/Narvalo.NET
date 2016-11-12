// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using static System.Diagnostics.Contracts.Contract;

    public static class Either
    {
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            Ensures(Result<Either<TLeft, TRight>>() != null);

            return Either<TLeft, TRight>.η(value);
        }

        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            Ensures(Result<Either<TLeft, TRight>>() != null);

            return Either<TLeft, TRight>.η(value);
        }
    }
}
