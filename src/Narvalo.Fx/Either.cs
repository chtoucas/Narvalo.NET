// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public static class Either
    {
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            Contract.Ensures(Contract.Result<Either<TLeft, TRight>>() != null);

            return Either<TLeft, TRight>.η(value);
        }

        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            Contract.Ensures(Contract.Result<Either<TLeft, TRight>>() != null);

            return Either<TLeft, TRight>.η(value);
        }
    }
}
