// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    public sealed class DecimalRounding : IDecimalRounding
    {
        // TODO: use a more sensible value.
        private const int MAX_DECIMALS = 10;

        private static readonly decimal[] s_Powers10 = new decimal[MAX_DECIMALS + 1]
        {
            1m,
            10m,
            100m,
            1000m,
            10000m,
            100000m,
            1000000m,
            10000000m,
            100000000m,
            1000000000m,
            10000000000m,
        };

        private static readonly decimal[] s_Epsilons = new decimal[MAX_DECIMALS + 1]
        {
            1m,
            0.1m,
            0.01m,
            0.001m,
            0.0001m,
            0.00001m,
            0.000001m,
            0.0000001m,
            0.00000001m,
            0.000000001m,
            0.0000000001m,
        };

        public decimal Round(decimal value)
            => Math.Round(value, 0, MidpointRounding.ToEven);

        public decimal Round(decimal value, int decimals)
            => Math.Round(value, decimals, MidpointRounding.ToEven);

        public static decimal Round(decimal value, int decimals, NumberRounding rounding)
        {
            Require.Range(0 <= decimals && decimals <= MAX_DECIMALS, nameof(decimals));

            return Round(s_Powers10[decimals] * value, rounding) * s_Epsilons[decimals];
        }

        // WARNING: It only works for representable values, ie those that has not been
        // silently rounded using the default rounding mode (MidpointRounding.ToEven).
        // Another problem is that we use operations that can produce non-representable values
        // (like -/+ 0.5m) which, then, are rounded automatically.
        // With PCL, we can not use Decimal.Round, instead we have Math.Round.
        public static decimal Round(decimal value, NumberRounding rounding)
        {
            switch (rounding)
            {
                case NumberRounding.Down:
                    return Decimal.Floor(value);

                case NumberRounding.Up:
                    return Decimal.Ceiling(value);

                case NumberRounding.TowardsZero:
                    // Equivalent to: x > 0 ? floor(x) : ceiling(x)
                    return Decimal.Truncate(value);

                case NumberRounding.AwayFromZero:
                    return value > 0 ? Decimal.Ceiling(value) : Decimal.Floor(value);

                case NumberRounding.HalfDown:
                    // For negative values, HalfDown ie equivalent to HalfAwayFromZero.
                    // This allows us to avoid to compute "value - 0.5m" which might be loosy.
                    // Another advantage is that we do not have to treat Decimal.Minvalue separately
                    // which would throw a StackOverflow exception.
                    return value > 0
                        ? Decimal.Ceiling(value - 0.5m)
                        : Math.Round(value, 0, MidpointRounding.AwayFromZero);

                case NumberRounding.HalfUp:
                    // For positive values, HalfUp ie equivalent to HalfAwayFromZero.
                    // This allows us to avoid to compute "value + 0.5m" which might be loosy.
                    // Another advantage is that we do not have to treat Decimal.MaxValue separately
                    // which would throw a StackOverflow exception.
                    return value > 0
                        ? Math.Round(value, 0, MidpointRounding.AwayFromZero)
                        : Decimal.Floor(value + 0.5m);

                case NumberRounding.HalfTowardsZero:
                    return value > 0 ? Decimal.Ceiling(value - 0.5m) : Decimal.Floor(value + 0.5m);

                case NumberRounding.HalfAwayFromZero:
                    // Equivalent to: x > 0 ? floor(x + .5) : ceiling(x - .5)
                    return Math.Round(value, 0, MidpointRounding.AwayFromZero);

                case NumberRounding.ToEven:
                    return Math.Round(value, 0, MidpointRounding.ToEven);

                case NumberRounding.ToOdd:
                    var n = Math.Round(value, 0, MidpointRounding.AwayFromZero);

                    if (value > 0)
                    {
                        return n - value == 0.5m && n % 2 == 0 ? --n : n;
                    }
                    else
                    {
                        return value - n == 0.5m && n % 2 == 0 ? ++n : n;
                    }

                default: throw Check.Unreachable("XXX");
            }
        }
    }
}
