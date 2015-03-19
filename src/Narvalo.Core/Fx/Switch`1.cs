// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public static class Switch<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        public static Switch<TLeft, T> Left<TLeft>(TLeft value)
        {
            Contract.Ensures(Contract.Result<Switch<TLeft, T>>() != null);

            return Switch<TLeft, T>.Left.η(value);
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        public static Switch<T, TRight> Right<TRight>(TRight value)
        {
            Contract.Ensures(Contract.Result<Switch<T, TRight>>() != null);

            return Switch<T, TRight>.Right.η(value);
        }
    }
}
