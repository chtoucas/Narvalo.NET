// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using Narvalo.Finance.Rounding;

    public static class MoneyFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount for which the number of decimal places is determined by the currency.
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        /// <param name="currency">The specific currency.</param>
        /// <param name="adjuster">The rounding adjuster.</param>
        public static Money Create(decimal amount, Currency currency, IRoundingAdjuster adjuster)
        {
            Require.NotNull(adjuster, nameof(adjuster));
            decimal value = adjuster.Round(amount, currency.DecimalPlaces);
            return Money.OfMajor(value, currency);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount given in minor units for which the number of decimal places is
        /// determined by the currency.
        /// </summary>
        /// <param name="minor">The decimal representing the amount of money in minor units.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="adjuster">The rounding adjuster.</param>
        public static Money CreateFromMinor(decimal minor, Currency currency, IRoundingAdjuster adjuster)
            => Create(currency.ConvertToMajor(minor), currency, adjuster);
    }
}
