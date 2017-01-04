// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    public enum NumberRounding
    {
        Down,

        Up,

        TowardsZero,

        AwayFromZero,

        // Unbalanced.
        HalfDown,

        // Unbalanced.
        // Advantage: for positive numbers, we just need to examine the first digit of the
        // fractional part.
        HalfUp,

        // Symmetric.
        HalfTowardsZero,

        // Also called commercial rounding.
        // Symmetric.
        // Advantage: we just need to examine the first digit of the fractional part.
        HalfAwayFromZero,

        // Also called bankers' rounding. IEEE 754 default rounding mode.
        // Symmetric / Balanced. Even-Odd biased.
        ToEven,

        // Symmetric / Balanced. Even-Odd biased.
        ToOdd,

        // Symmetric / Balanced. Even-Odd unbiased.
        // Disadvantage: difficult to reproduce.
        Stochastic,
    }
}
