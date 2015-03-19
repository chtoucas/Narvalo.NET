// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public static class Either<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "Type inference won't help here.")]
        public static Either<TLeft, T> Left<TLeft>(TLeft value)
        {
            Contract.Ensures(Contract.Result<Either<TLeft, T>>() != null);

            return new Either<TLeft, T>.Left(value);
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "Type inference won't help here.")]
        public static Either<T, TRight> Right<TRight>(TRight value)
        {
            Contract.Ensures(Contract.Result<Either<T, TRight>>() != null);

            return new Either<T, TRight>.Right(value);
        }
    }
}
