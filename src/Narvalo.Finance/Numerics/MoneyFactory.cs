// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    public static class MoneyFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount for which the number of decimal places is determined by the currency.
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        /// <param name="currency">The specific currency.</param>
        /// <param name="rounding">The rounding operator.</param>
        public static Money Create(decimal amount, Currency currency, IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));
            var ramount = rounding.Round(amount, currency.DecimalPlaces);
            return Money.OfMajor(ramount, currency);
        }
    }
}
