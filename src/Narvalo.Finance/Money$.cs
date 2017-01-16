// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using static Narvalo.Finance.MoneyCalculator;

    public static class MoneyExtensions
    {
        public static Money Plus(this Money @this, decimal amount, MidpointRounding mode)
            => Add(@this, amount, mode);

        public static Money Plus(this Money @this, Money other, MidpointRounding mode)
            => Add(@this, other, mode);

        public static Money Minus(this Money @this, decimal amount, MidpointRounding mode)
            => Subtract(@this, amount, mode);

        public static Money Minus(this Money @this, Money other, MidpointRounding mode)
            => Subtract(@this, other, mode);

        public static Money MultiplyBy(this Money @this, decimal multiplier, MidpointRounding mode)
            => Multiply(@this, multiplier, mode);

        public static Money DivideBy(this Money @this, decimal divisor, MidpointRounding mode)
            => Divide(@this, divisor, mode);

        public static Money Remainder(this Money @this, decimal divisor, MidpointRounding mode)
            => Modulus(@this, divisor, mode);
    }
}
