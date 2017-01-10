// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    using System;

    public static partial class RoundingAdjusters
    {
        public static decimal Round(decimal value, int decimalPlaces, RoundingMode mode)
        {
            if (mode == RoundingMode.ToEven)
            {
                return ToEven(value, decimalPlaces);
            }
            else if (mode == RoundingMode.AwayFromZero)
            {
                return HalfAwayFromZero(value, decimalPlaces);
            }

            return decimalPlaces == 0
                ? Round(value, mode)
                : DefaultRoundingAdjuster.Unscale(
                    Round(
                        DefaultRoundingAdjuster.Scale(value, decimalPlaces),
                        mode),
                    decimalPlaces);
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
                    return HalfAwayFromZero(value, 0);
                case RoundingMode.ToEven:
                    return ToEven(value, 0);
            }

            // Rounding modes not part of IEEE 754.
            switch (mode)
            {
                case RoundingMode.AwayFromZero:
                    return value > 0m ? Decimal.Ceiling(value) : Decimal.Floor(value);

                case RoundingMode.HalfDown:
                    return HalfDown(value);

                case RoundingMode.HalfUp:
                    return HalfUp(value);

                case RoundingMode.HalfTowardsZero:
                    return value > 0
                        ? RoundHalfTowardsZeroForPositiveValue(value)
                        : RoundHalfTowardsZeroForNegativeValue(value);

                case RoundingMode.ToOdd:
                    var n = HalfAwayFromZero(value, 0);

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

        #region Rounding modes defined in IEEE 754. Unsurprisingly, .NET provides native support for them.

        public static decimal Down(decimal value) => Decimal.Floor(value);

        public static decimal Up(decimal value) => Decimal.Ceiling(value);

        public static decimal TowardsZero(decimal value) => Decimal.Truncate(value);

        public static decimal HalfAwayFromZero(decimal value)
            => Math.Round(value, MidpointRounding.AwayFromZero);

        public static decimal HalfAwayFromZero(decimal value, int decimalPlaces)
            => Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);

        public static decimal ToEven(decimal value)
            => Math.Round(value, MidpointRounding.ToEven);

        public static decimal ToEven(decimal value, int decimalPlaces)
            => Math.Round(value, decimalPlaces, MidpointRounding.ToEven);

        public static IRoundingAdjuster ToEvenAdjuster { get { throw new NotImplementedException(); } }

        #endregion

        #region Rounding modes not part of IEEE 754.

        public static decimal AwayFromZero(decimal value)
        {
            throw new NotImplementedException();
        }

        public static decimal HalfDown(decimal value, int decimalPlaces)
        {
            if (value == 0m) { return 0m; }
            return decimalPlaces == 0
                ? HalfDown(value)
                : DefaultRoundingAdjuster.Unscale(
                    HalfDown(
                        DefaultRoundingAdjuster.Scale(value, decimalPlaces)),
                    decimalPlaces);
        }

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

        public static decimal HalfUp(decimal value, int decimalPlaces)
        {
            if (value == 0m) { return 0m; }
            return decimalPlaces == 0
                ? HalfUp(value)
                : DefaultRoundingAdjuster.Unscale(
                    HalfUp(
                        DefaultRoundingAdjuster.Scale(value, decimalPlaces)),
                    decimalPlaces);
        }

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

        #endregion
    }
}
