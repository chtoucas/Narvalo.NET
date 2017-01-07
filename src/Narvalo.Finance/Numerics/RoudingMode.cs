// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    // Directed rouding
    // ----------------
    //
    // - Down, aka "rounding towards minus infinity"       -infty <-
    // - Up, aka "rounding towards plus infinity"                      -> +infty
    // - TowardsZero, aka "rounding away from infinity"           -> 0 <-
    // - AwayFromZero, aka "rounding towards infinity"            <- 0 ->
    // Among these four modes, only the last one is not part of IEEE 754.
    //
    // Mathematically speaking:
    // - Down,         floor(x) = n where n <= x < n + 1. En notation française, [x]
    // - Up,           ceiling(x) = n where n - 1 < x <= n
    // - TowardsZero,  truncate(x) = sign(x) [|x|]
    //              or x > 0 ? floor(x) : ceiling(x)
    // - AwayFromZero, n = - sign(x) [-|x|]
    // NB:
    // - for x > 0, Down = TowardsZero and Up = AwayFromZero
    // - for x < 0, Down = AwayFromZero and Up = TowardsZero
    // - floor(-x) = - ceiling(x)
    //
    //       Down Up   TowardsZero AwayFromZero
    //  1.5   1    2    1            2
    //  0.5   0    1    0            1
    // -0.5  -1    0    0           -1
    // -1.5  -2   -1   -1           -2
    //
    // Rounding to the nearest integer
    // -------------------------------
    //
    // When the fractional part is half-way of two integers, we need a tie-breaking rule.
    //
    // - HalfDown,         n = ceiling(x - 0.5)
    // - HalfUp,           n = [x + 0.5]
    // - HalfTowardsZero,  n = - sign(x) [-|x| + 0.5]
    //                  or n = x > 0 ? ceiling(x - .5) : floor(x + .5)
    // - HalfAwayFromZero, n = sign(x) [|x| + 0.5]
    //                  or n = x > 0 ? floor(x + .5) : ceiling(x - .5)
    // - ToEven,
    // - ToOdd,
    // Among these six modes, only HalfAwayFromZero and ToEven are part of IEEE 754.
    //
    //       HalfDown HalfUp HalfTowardsZero HalfAwayFromZero ToEven ToOdd
    //  1.5   1        2      1               2                2      1
    //  0.5   0        1      0               1                0      1
    // -0.5  -1        0      0              -1                0     -1
    // -1.5  -2       -1     -1              -2               -2     -1
    //
    // Notes:
    // - for HalfUp and positive numbers, we just need to examine the first digit of the
    //   fractional part.
    // - HalfAwayFromZero, aka called commercial rounding,
    //   we just need to examine the first digit of the fractional part.
    // - ToEven, aka bankers' rounding. IEEE 754 default rounding mode.
    //
    // A good rule is
    // - symmetric, it treats symmetrically positive and negative numbers
    // - balanced, the effect of rounding is statistically cancelled
    // - reproducible
    // This rules out HalfDown and HalfUp which are not symmetric.
    //
    // Remark: Integer division rounds toward zero.
    public enum RoundingMode
    {
        Down, // roundTowardNegative (IEEE 754)

        Up, // roundTowardPositive (IEEE 754)

        TowardsZero, // roundTowardZero (IEEE 754)

        AwayFromZero,

        HalfDown,

        HalfUp,

        HalfTowardsZero,

        HalfAwayFromZero, // roundTiesToAway (IEEE 754)

        ToEven, // roundTiesToEven (IEEE 754)

        ToOdd,
    }
}
