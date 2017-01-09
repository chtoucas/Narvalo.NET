// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Numerics;

    public static class CurrencyExtensions
    {
        internal static decimal Round(this Currency @this, decimal amount, MidpointRounding mode)
            => mode.Round(amount, @this.DecimalPlaces);

        /// <summary>
        /// Converts an amount to a value expressed in minor units.
        /// </summary>
        /// <remarks>We expect the amount to be normalized.</remarks>
        /// <exception cref="OverflowException">Thrown if the amount is too large to fit into
        /// the Int64 range.</exception>
        internal static long ConvertToMinorUnits(this Currency @this, decimal major)
            => Convert.ToInt64(@this.Factor * major);

        public static long ConvertToMinorUnits(this Currency @this, decimal amount, MidpointRounding mode)
            => ConvertToMinorUnits(@this, Round(@this, amount, mode));

        public static long? TryConvertToMinorUnits(this Currency @this, decimal amount, MidpointRounding mode)
        {
            decimal minor = @this.Factor * Round(@this, amount, mode);
            if (minor < Int64.MinValue || minor > Int64.MaxValue) { return null; }
            return Convert.ToInt64(minor);
        }

        public static bool TryConvertToMinorUnits(this Currency @this, decimal amount, MidpointRounding mode, out long result)
        {
            long? minor = TryConvertToMinorUnits(@this, amount, mode);
            result = minor ?? (amount > 0 ? Int64.MaxValue : Int64.MinValue);
            return minor.HasValue;
        }

        public static decimal ToMajorUnits(this Currency @this, long amount) => @this.Epsilon * amount;
    }
}
