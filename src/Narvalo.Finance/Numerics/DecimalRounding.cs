// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    public sealed class DecimalRounding : IDecimalRounding
    {
        private const int MAX_SCALE = 9;

        private const decimal
            MAX_1 = Decimal.MaxValue / 10,
            MAX_2 = MAX_1 / 10,
            MAX_3 = MAX_2 / 10,
            MAX_4 = MAX_3 / 10,
            MAX_5 = MAX_4 / 10,
            MAX_6 = MAX_5 / 10,
            MAX_7 = MAX_6 / 10,
            MAX_8 = MAX_7 / 10,
            MAX_9 = MAX_8 / 10;

        private static readonly decimal[] s_MaxValues = new decimal[MAX_SCALE]
        {
            MAX_1,
            MAX_2,
            MAX_3,
            MAX_4,
            MAX_5,
            MAX_6,
            MAX_7,
            MAX_8,
            MAX_9
        };

        private static readonly uint[] s_Powers10 = new uint[MAX_SCALE]
        {
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000
        };

        private static readonly decimal[] s_Epsilons = new decimal[MAX_SCALE]
        {
            0.1m,
            0.01m,
            0.001m,
            0.0001m,
            0.00001m,
            0.000001m,
            0.0000001m,
            0.00000001m,
            0.000000001m
        };

        public DecimalRounding() : this(NumberRounding.ToEven) { }

        public DecimalRounding(NumberRounding rounding)
        {
            Rounding = rounding;
        }

        public NumberRounding Rounding { get; }

        #region IDecimalRounding interface.

        public decimal Round(decimal value) => Round(value, Rounding);

        public decimal Round(decimal value, int decimals)
        {
            if (Rounding == NumberRounding.ToEven)
            {
                return Math.Round(value, decimals, MidpointRounding.ToEven);
            }
            else if (Rounding == NumberRounding.AwayFromZero)
            {
                return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
            }
            else if (decimals == 0)
            {
                return Round(value, Rounding);
            }
            else
            {
                CheckRange(value, decimals);
                return Round(s_Powers10[decimals - 1] * value, Rounding) * s_Epsilons[decimals - 1];
            }
        }

        #endregion

        public static decimal Round(decimal value, NumberRounding rounding)
        {
            if (value == 0m) { return 0m; }

            // Rounding modes defined in IEEE 754.
            // Unsurprisingly, .NET provides native support for them.
            switch (rounding)
            {
                case NumberRounding.Down:
                    return Decimal.Floor(value);
                case NumberRounding.Up:
                    return Decimal.Ceiling(value);
                case NumberRounding.TowardsZero:
                    return Decimal.Truncate(value);
                case NumberRounding.HalfAwayFromZero:
                    return Math.Round(value, 0, MidpointRounding.AwayFromZero);
                case NumberRounding.ToEven:
                    return Math.Round(value, 0, MidpointRounding.ToEven);
            }

            // Rounding modes not part of IEEE 754.
            switch (rounding)
            {
                case NumberRounding.AwayFromZero:
                    return value > 0m ? Decimal.Ceiling(value) : Decimal.Floor(value);

                case NumberRounding.HalfDown:
                    return RoundHalfDown(value);

                case NumberRounding.HalfUp:
                    return RoundHalfUp(value);

                case NumberRounding.HalfTowardsZero:
                    return value > 0
                        ? RoundHalfTowardsZeroForPositiveValue(value)
                        : RoundHalfTowardsZeroForNegativeValue(value);

                case NumberRounding.ToOdd:
                    var n = Math.Round(value, 0, MidpointRounding.AwayFromZero);

                    if (value > 0m)
                    {
                        return value - n == -0.5m && n % 2 == 0 ? --n : n;
                    }
                    else
                    {
                        return value - n == 0.5m && n % 2 == 0 ? ++n : n;
                    };

                default: throw Check.Unreachable("XXX");
            }
        }

        internal static decimal RoundHalfDown(decimal value, int decimals)
        {
            if (value == 0m) { return 0m; }

            if (decimals == 0)
            {
                return RoundHalfDown(value);
            }
            else
            {
                CheckRange(value, decimals);
                return RoundHalfDown(s_Powers10[decimals - 1] * value) * s_Epsilons[decimals - 1];
            }
        }

        internal static decimal RoundHalfUp(decimal value, int decimals)
        {
            if (value == 0m) { return 0m; }

            if (decimals == 0)
            {
                return RoundHalfUp(value);
            }
            else
            {
                CheckRange(value, decimals);
                return RoundHalfUp(s_Powers10[decimals - 1] * value) * s_Epsilons[decimals - 1];
            }
        }

        #region Helpers.

        // For positive values, HalfDown is equivalent to HalfTowardsZero.
        // For negative values, HalfDown is equivalent to HalfAwayFromZero.
        // If there were no risks, we could simply compute Decimal.Ceiling(value - 0.5m),
        // but "value - 0.5m" might be rounded automatically (nearest to even) if it is not
        // representable. Another advantage is that we do not have to treat Decimal.Minvalue
        // separately which would throw a StackOverflow exception.
        private static decimal RoundHalfDown(decimal value)
            => value > 0m
            ? RoundHalfTowardsZeroForPositiveValue(value)
            : Math.Round(value, 0, MidpointRounding.AwayFromZero);

        // For positive values, HalfUp is equivalent to HalfAwayFromZero.
        // For negative values, HalfUp is equivalent to HalfTowardsZero.
        // If there were no risks, we could simply compute Decimal.Floor(value + 0.5m),
        // but "value + 0.5m" might be rounded automatically (nearest to even) if it is not
        // representable. Another advantage is that we do not have to treat Decimal.MaxValue
        // separately which would throw a StackOverflow exception.
        private static decimal RoundHalfUp(decimal value)
            => value > 0m
            ? Math.Round(value, 0, MidpointRounding.AwayFromZero)
            : RoundHalfTowardsZeroForNegativeValue(value);

        private static decimal RoundHalfTowardsZeroForPositiveValue(decimal value)
        {
            var n = Decimal.Floor(value);
            // If 'value' is not a midpoint, we return the nearest integer.
            return value - n == 0.5m ? n : Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        private static decimal RoundHalfTowardsZeroForNegativeValue(decimal value)
        {
            var n = Decimal.Ceiling(value);
            // If 'value' is not a midpoint, we return the nearest integer.
            return value - n == -0.5m ? n : Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        private static void CheckRange(decimal value, int decimals)
        {
            Enforce.Range(1 <= decimals && decimals <= MAX_SCALE, nameof(decimals));

            decimal maxValue = s_MaxValues[decimals - 1];
            Enforce.Range(-maxValue <= value && value <= maxValue, nameof(value));
        }

        #endregion
    }
}
