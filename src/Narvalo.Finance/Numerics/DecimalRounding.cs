// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    public enum DecimalRounding
    {
        // Math.Floor(); partie entière (n <= x < n +1); partie fractionnaire ({x} = x - |x|)
        Down,

        // Math.Ceiling(); n - 1 < x <= n.
        Up,

        // Math.Truncate()
        Truncate,

        TowardZero,

        // Math.Round(MidpointRounding.AwayFromZero)
        AwayFromZero,

        // Bankers' Rounding.
        // Advantage: it is symmetric (negative and positive numbers are treated equally);
        // we can divide the result by two without losing precision; it is unbalanced compare to Down
        // or Up when it comes to cumulative computations followed by approximations.
        // Math.Round(MidpointRounding.ToEven)
        ToEven,

        ToOdd,

        Nearest,
    }
}
