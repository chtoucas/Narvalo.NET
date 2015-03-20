// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;

    public static class Switch
    {
        public static Switch<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

            return Switch<TLeft, TRight>.η(value);
        }

        public static Switch<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            Contract.Ensures(Contract.Result<Switch<TLeft, TRight>>() != null);

            return Switch<TLeft, TRight>.η(value);
        }
    }
}
