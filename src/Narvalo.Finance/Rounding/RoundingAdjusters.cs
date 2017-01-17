// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static partial class RoundingAdjusters
    {
        #region Constants.

        // This limit is rather artificial, but this should be OK for our use cases.
        // NB: This limit is not enforced for ToEven and HalAwayFromZero, in which cases
        // we simply use the default maximum scale for decimals.
        private const int MAX_SCALE = 9;
        private const int MAX_DECIMAL_SCALE = 28;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "[Ignore] Weird, these constants are used to initialize s_MaxValues.")]
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

        #endregion

        public static decimal Round(decimal value, int decimalPlaces, RoundingMode mode)
        {
            // Rounding modes defined in IEEE 754. Unsurprisingly, .NET provides native support for them.
            switch (mode)
            {
                case RoundingMode.Down: return Down(value, decimalPlaces);
                case RoundingMode.Up: return Up(value, decimalPlaces);
                case RoundingMode.TowardsZero: return TowardsZero(value, decimalPlaces);
                case RoundingMode.HalfAwayFromZero: return HalfAwayFromZero(value, decimalPlaces);
                case RoundingMode.ToEven: return ToEven(value, decimalPlaces);
            }

            // Rounding modes not part of IEEE 754.
            switch (mode)
            {
                case RoundingMode.AwayFromZero: return AwayFromZero(value, decimalPlaces);
                case RoundingMode.HalfDown: return HalfDown(value, decimalPlaces);
                case RoundingMode.HalfUp: return HalfUp(value, decimalPlaces);
                case RoundingMode.HalfTowardsZero: return HalfTowardsZero(value, decimalPlaces);
                case RoundingMode.ToOdd: return ToOdd(value, decimalPlaces);

                default: throw Check.Unreachable("XXX");
            }
        }

        public static decimal Round(decimal value, RoundingMode mode)
        {
            if (value == 0m) { return 0m; }

            // Rounding modes defined in IEEE 754. Unsurprisingly, .NET provides native support for them.
            switch (mode)
            {
                case RoundingMode.Down: return Down(value);
                case RoundingMode.Up: return Up(value);
                case RoundingMode.TowardsZero: return TowardsZero(value);
                case RoundingMode.HalfAwayFromZero: return HalfAwayFromZero(value);
                case RoundingMode.ToEven: return ToEven(value);
            }

            // Rounding modes not part of IEEE 754.
            switch (mode)
            {
                case RoundingMode.AwayFromZero: return AwayFromZero(value);
                case RoundingMode.HalfDown: return HalfDown(value);
                case RoundingMode.HalfUp: return HalfUp(value);
                case RoundingMode.HalfTowardsZero: return HalfTowardsZero(value);
                case RoundingMode.ToOdd: return ToOdd(value);

                default: throw Check.Unreachable("XXX");
            }
        }

        #region Rounding to an integer.

        public static decimal AwayFromZero(decimal value)
            => value > 0m ? Decimal.Ceiling(value) : Decimal.Floor(value);

        public static decimal Down(decimal value) => Decimal.Floor(value);

        public static decimal HalfAwayFromZero(decimal value) => Math.Round(value, MidpointRounding.AwayFromZero);

        // For positive values, HalfDown is equivalent to HalfTowardsZero.
        // For negative values, HalfDown is equivalent to HalfAwayFromZero.
        // If there were no risks, we could simply compute Decimal.Ceiling(value - 0.5m),
        // but "value - 0.5m" might be rounded automatically (nearest to even) if it is not
        // representable. Another advantage is that we do not have to treat Decimal.Minvalue
        // separately which would throw a StackOverflow exception.
        public static decimal HalfDown(decimal value)
            => value > 0m
            ? RoundHalfTowardsZeroForPositiveValue(value)
            : HalfAwayFromZero(value, 0);

        public static decimal HalfTowardsZero(decimal value)
            => value > 0m
            ? RoundHalfTowardsZeroForPositiveValue(value)
            : RoundHalfTowardsZeroForNegativeValue(value);

        // For positive values, HalfUp is equivalent to HalfAwayFromZero.
        // For negative values, HalfUp is equivalent to HalfTowardsZero.
        // If there were no risks, we could simply compute Decimal.Floor(value + 0.5m),
        // but "value + 0.5m" might be rounded automatically (nearest to even) if it is not
        // representable. Another advantage is that we do not have to treat Decimal.MaxValue
        // separately which would throw a StackOverflow exception.
        public static decimal HalfUp(decimal value)
            => value > 0m
            ? HalfAwayFromZero(value, 0)
            : RoundHalfTowardsZeroForNegativeValue(value);

        public static decimal ToEven(decimal value) => Math.Round(value, MidpointRounding.ToEven);

        public static decimal ToOdd(decimal value)
        {
            var n = HalfAwayFromZero(value, 0);

            if (value > 0m)
            {
                return value - n == -0.5m && n % 2 == 0 ? --n : n;
            }
            else
            {
                return value - n == 0.5m && n % 2 == 0 ? ++n : n;
            };
        }

        public static decimal TowardsZero(decimal value) => Decimal.Truncate(value);

        public static decimal Up(decimal value) => Decimal.Ceiling(value);

        #endregion

        #region Rounding to a number of decimal digits.

        public static decimal AwayFromZero(decimal value, int decimalPlaces)
            => RoundImpl(value, decimalPlaces, AwayFromZero);

        public static decimal Down(decimal value, int decimalPlaces)
            => RoundImpl(value, decimalPlaces, Down);

        public static decimal HalfAwayFromZero(decimal value, int decimalPlaces)
            => Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);

        public static decimal HalfDown(decimal value, int decimalPlaces)
            => RoundImpl(value, decimalPlaces, HalfDown);

        public static decimal HalfTowardsZero(decimal value, int decimalPlaces)
            => RoundImpl(value, decimalPlaces, HalfTowardsZero);

        public static decimal HalfUp(decimal value, int decimalPlaces)
            => RoundImpl(value, decimalPlaces, HalfUp);

        public static decimal TowardsZero(decimal value, int decimalPlaces)
            => RoundImpl(value, decimalPlaces, TowardsZero);

        public static decimal Up(decimal value, int decimalPlaces)
            => RoundImpl(value, decimalPlaces, Up);

        public static decimal ToEven(decimal value, int decimalPlaces)
            => Math.Round(value, decimalPlaces, MidpointRounding.ToEven);

        public static decimal ToOdd(decimal value, int decimalPlaces)
            => RoundImpl(value, decimalPlaces, ToOdd);

        #endregion

        #region Helpers.

        private static decimal RoundHalfTowardsZeroForPositiveValue(decimal value)
        {
            var n = Decimal.Floor(value);
            // If 'value' is not a midpoint, we return the nearest integer.
            return value - n == 0.5m ? n : HalfAwayFromZero(value, 0);
        }

        private static decimal RoundHalfTowardsZeroForNegativeValue(decimal value)
        {
            var n = Decimal.Ceiling(value);
            // If 'value' is not a midpoint, we return the nearest integer.
            return value - n == -0.5m ? n : HalfAwayFromZero(value, 0);
        }

        private static decimal RoundImpl(decimal value, int decimalPlaces, Func<decimal, decimal> round)
        {
            if (value == 0m) { return 0m; }
            return decimalPlaces == 0
                ? round.Invoke(value)
                : Downscale(
                    round.Invoke(Upscale(value, decimalPlaces)),
                    decimalPlaces);
        }

        private static decimal Upscale(decimal value, int decimalPlaces)
        {
            Require.Range(1 <= decimalPlaces && decimalPlaces <= MAX_SCALE, nameof(decimalPlaces));

            decimal maxValue = s_MaxValues[decimalPlaces - 1];
            Enforce.Range(-maxValue <= value && value <= maxValue, nameof(value));

            return s_Powers10[decimalPlaces - 1] * value;
        }

        private static decimal Downscale(decimal value, int decimalPlaces)
            => s_Epsilons[decimalPlaces - 1] * value;

        #endregion
    }
}
