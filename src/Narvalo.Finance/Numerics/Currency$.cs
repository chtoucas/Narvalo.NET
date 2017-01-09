// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    public static class CurrencyExtensions
    {
        internal static decimal Round(this Currency @this, decimal amount, IDecimalRounding rounding)
            => rounding.Round(amount, @this.DecimalPlaces);

        public static long ConvertToMinorUnits(this Currency @this, decimal amount, IDecimalRounding rounding)
            => @this.ConvertToMinorUnits(Round(@this, amount, rounding));

        public static long? TryConvertToMinorUnits(
            this Currency @this,
            decimal amount,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));
            decimal minor = @this.Factor * Round(@this, amount, rounding);
            if (minor < Int64.MinValue || minor > Int64.MaxValue) { return null; }
            return Convert.ToInt64(minor);
        }

        public static bool TryConvertToMinorUnits(
            this Currency @this,
            decimal amount,
            IDecimalRounding rounding,
            out long result)
        {
            Expect.NotNull(rounding);
            long? minor = TryConvertToMinorUnits(@this, amount, rounding);
            result = minor ?? (amount > 0 ? Int64.MaxValue : Int64.MinValue);
            return minor.HasValue;
        }
    }
}
