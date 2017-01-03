// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    // See https://en.wikipedia.org/wiki/Rounding
    // In .NET:
    // - ToEven, aka Bankers' Rounding.
    //   Advantage: it is symmetric (negative and positive numbers are treated equally);
    //   we can divide the result by two without losing precision; it is unbalanced compare to Down
    //  or Up when it comes to cumulative computations followed by approximations.
    // - AwayFromZero,
    // Stochastic?
    public enum DecimalRounding
    {
        Down,
        Up,
        TowardZero,
        AwayFromZero,
        ToEven,
        ToOdd,
        Nearest,
    }
}
