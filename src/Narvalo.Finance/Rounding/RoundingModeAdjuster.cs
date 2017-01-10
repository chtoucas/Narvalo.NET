// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public abstract class DefaultRoundingAdjuster : IRoundingAdjuster
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

        public abstract decimal RoundCore(decimal value);

        public decimal Round(decimal value)
        {
            if (value == 0m) { return 0m; }
            return RoundCore(value);
        }

        public decimal Round(decimal value, int decimalPlaces)
        {
            if (value == 0m) { return 0m; }

            if (decimalPlaces == 0)
            {
                return RoundCore(value);
            }
            else
            {
                decimal scaled = Scale(value, decimalPlaces);
                decimal rounded = RoundCore(scaled);
                return Unscale(rounded, decimalPlaces);
            }
        }

        internal static decimal Scale(decimal value, int decimalPlaces)
        {
            Require.Range(1 <= decimalPlaces && decimalPlaces <= MAX_SCALE, nameof(decimalPlaces));

            decimal maxValue = s_MaxValues[decimalPlaces - 1];
            Enforce.Range(-maxValue <= value && value <= maxValue, nameof(value));

            return s_Powers10[decimalPlaces - 1] * value;
        }

        internal static decimal Unscale(decimal value, int decimalPlaces)
            => s_Epsilons[decimalPlaces - 1] * value;
    }
}
