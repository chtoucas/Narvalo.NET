// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using static System.Diagnostics.Contracts.Contract;

    public static class Switch
    {
        public static Switch<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            Ensures(Result<Switch<TLeft, TRight>>() != null);

            return Switch<TLeft, TRight>.η(value);
        }

        public static Switch<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            Ensures(Result<Switch<TLeft, TRight>>() != null);

            return Switch<TLeft, TRight>.η(value);
        }
    }
}
