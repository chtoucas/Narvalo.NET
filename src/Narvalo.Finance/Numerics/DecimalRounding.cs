// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public sealed class DecimalRounding : IDecimalRounding
    {
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

        public DecimalRounding() : this(RoundingMode.ToEven) { }

        public DecimalRounding(RoundingMode mode)
        {
            Mode = mode;
        }

        public RoundingMode Mode { get; }

        #region IDecimalRounding interface.

        public decimal Round(decimal value, int decimalPlaces) => Round(value, decimalPlaces, Mode);

        #endregion

        public static decimal Round(decimal value, int decimalPlaces, RoundingMode mode)
        {
            if (mode == RoundingMode.ToEven)
            {
                return RoundToEven(value, decimalPlaces);
            }
            else if (mode == RoundingMode.AwayFromZero)
            {
                return RoundHalfAwayFromZero(value, decimalPlaces);
            }

            return decimalPlaces == 0
                ? Round(value, mode)
                : Unscale(Round(Scale(value, decimalPlaces), mode), decimalPlaces);
        }

        public static decimal Round(decimal value, RoundingMode mode)
        {
            if (value == 0m) { return 0m; }

            // Rounding modes defined in IEEE 754.
            // Unsurprisingly, .NET provides native support for them.
            switch (mode)
            {
                case RoundingMode.Down:
                    return Decimal.Floor(value);
                case RoundingMode.Up:
                    return Decimal.Ceiling(value);
                case RoundingMode.TowardsZero:
                    return Decimal.Truncate(value);
                case RoundingMode.HalfAwayFromZero:
                    return RoundHalfAwayFromZero(value, 0);
                case RoundingMode.ToEven:
                    return RoundToEven(value, 0);
            }

            // Rounding modes not part of IEEE 754.
            switch (mode)
            {
                case RoundingMode.AwayFromZero:
                    return value > 0m ? Decimal.Ceiling(value) : Decimal.Floor(value);

                case RoundingMode.HalfDown:
                    return RoundHalfDown(value);

                case RoundingMode.HalfUp:
                    return RoundHalfUp(value);

                case RoundingMode.HalfTowardsZero:
                    return value > 0
                        ? RoundHalfTowardsZeroForPositiveValue(value)
                        : RoundHalfTowardsZeroForNegativeValue(value);

                case RoundingMode.ToOdd:
                    var n = RoundHalfAwayFromZero(value, 0);

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static decimal RoundHalfDown(decimal value, int decimalPlaces)
        {
            if (value == 0m) { return 0m; }
            return decimalPlaces == 0
                ? RoundHalfDown(value)
                : Unscale(RoundHalfDown(Scale(value, decimalPlaces)), decimalPlaces);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static decimal RoundHalfUp(decimal value, int decimalPlaces)
        {
            if (value == 0m) { return 0m; }
            return decimalPlaces == 0
                ? RoundHalfUp(value)
                : Unscale(RoundHalfUp(Scale(value, decimalPlaces)), decimalPlaces);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static decimal RoundToEven(decimal value, int decimalPlaces)
        {
            Demand.Range(0 <= decimalPlaces && decimalPlaces <= MAX_DECIMAL_SCALE);
            return Math.Round(value, decimalPlaces, MidpointRounding.ToEven);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static decimal RoundHalfAwayFromZero(decimal value, int decimalPlaces)
        {
            Demand.Range(0 <= decimalPlaces && decimalPlaces <= MAX_DECIMAL_SCALE);
            return Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
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
            : RoundHalfAwayFromZero(value, 0);

        // For positive values, HalfUp is equivalent to HalfAwayFromZero.
        // For negative values, HalfUp is equivalent to HalfTowardsZero.
        // If there were no risks, we could simply compute Decimal.Floor(value + 0.5m),
        // but "value + 0.5m" might be rounded automatically (nearest to even) if it is not
        // representable. Another advantage is that we do not have to treat Decimal.MaxValue
        // separately which would throw a StackOverflow exception.
        private static decimal RoundHalfUp(decimal value)
            => value > 0m
            ? RoundHalfAwayFromZero(value, 0)
            : RoundHalfTowardsZeroForNegativeValue(value);

        private static decimal RoundHalfTowardsZeroForPositiveValue(decimal value)
        {
            var n = Decimal.Floor(value);
            // If 'value' is not a midpoint, we return the nearest integer.
            return value - n == 0.5m ? n : RoundHalfAwayFromZero(value, 0);
        }

        private static decimal RoundHalfTowardsZeroForNegativeValue(decimal value)
        {
            var n = Decimal.Ceiling(value);
            // If 'value' is not a midpoint, we return the nearest integer.
            return value - n == -0.5m ? n : RoundHalfAwayFromZero(value, 0);
        }

        private static decimal Scale(decimal value, int decimalPlaces)
        {
            Require.Range(1 <= decimalPlaces && decimalPlaces <= MAX_SCALE, nameof(decimalPlaces));

            decimal maxValue = s_MaxValues[decimalPlaces - 1];
            Enforce.Range(-maxValue <= value && value <= maxValue, nameof(value));

            return s_Powers10[decimalPlaces - 1] * value;
        }

        private static decimal Unscale(decimal value, int decimalPlaces)
            => s_Epsilons[decimalPlaces - 1] * value;

        #endregion
    }
}
