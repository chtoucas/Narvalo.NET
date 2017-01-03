// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    // See https://en.wikipedia.org/wiki/Rounding.
    //
    // Input Down Up   TowardsZero AwayFromZero Nearest
    // 1.6    1    2    1           2            2
    // 1.4                                       1
    // 0.6    0    1    0           1
    // 0.4                                       0
    // -0.4  -1    0    0           -1
    // -0.6                                     -1
    // -1.4  -2   -1   -1           -2
    // -1.6                                     -2
    //
    // Input HalfDown HalfUp HalfTowardsZero HalfAwayFromZero ToEven ToOdd
    // 1.5    1        2      1               2                2      1
    // 0.5    0        1      0               1                0      1
    // -0.5  -1        0      0              -1                0     -1
    // -1.5  -2       -1     -1              -2               -2     -1
    public enum NumberRounding
    {
        #region Directed roundings.

        // Also called "rounding towards minus infinity".
        // Behind the scene, use Math.Floor().
        // n <= x < n + 1 denoted by [x] (notation française).
        Down,

        // Also called "rounding towards plus infinity".
        // Behind the scene, use Math.Ceiling().
        // n - 1 < x <= n.
        Up,

        // Also called "rounding away from infinity".
        // Behind the scene, use Math.Truncate().
        // n = sign(x) [|x|]
        TowardsZero,

        // Also called "rounding towards infinity".
        // n = - sign(x) [-|x|]
        AwayFromZero,

        #endregion

        #region Rounding to the nearest integer.

        // When the fractional part is half-way of two integers, we need a tie-breaking rule.
        // A rule is said to be balanced if the effect of rounding is statistically cancelled.
        // A rule is said to be symmetric if it treats symmetrically positive and negative numbers.

        // Unbalanced.
        // n = [x - 0.5]
        HalfDown,

        // Unbalanced.
        // Advantage: for positive numbers, we just need to examine the first digit of the
        // fractional part.
        // n = [x + 0.5]
        HalfUp,

        // Symmetric.
        // n = - sign(x) [-|x| + 0.5]
        HalfTowardsZero,

        // Also called commercial rounding.
        // Symmetric.
        // Advantage: we just need to examine the first digit of the fractional part.
        // Behind the scene, use Math.Round(MidpointRounding.AwayFromZero).
        // n = sign(x) [|x| + 0.5]
        HalfAwayFromZero,

        // Also called bankers' rounding. IEEE 754 default rounding mode.
        // Symmetric / Balanced. Even-Odd biased.
        // Advantage: we can divide the result by two without losing precision.
        // Behind the scene, use Math.Round(MidpointRounding.ToEven).
        ToEven,

        // Symmetric / Balanced. Even-Odd biased.
        ToOdd,

        // Symmetric / Balanced. Even-Odd unbiased.
        // Disadvantage: difficult to reproduce.
        Stochastic,

        #endregion
    }
}
