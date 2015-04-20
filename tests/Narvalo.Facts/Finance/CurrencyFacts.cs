// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Currencies;
    using Xunit;

    public static class CurrencyFacts
    {
        #region Euro

        [Fact]
        public static void Equality_ReturnsTrue_WhenComparingAllEuroInstances()
        {
            // Act & Assert
            Assert.True(EUR.Currency.Equals(Currency.Euro));

            Assert.True(Currency.Euro.Equals(Currency.Of("EUR")));
            Assert.True(Currency.Euro.Equals(EUR.Currency));

            Assert.True(EUR.Currency == Currency.Euro);
            Assert.True(Currency.Euro == Currency.Of("EUR"));

            Assert.True(Object.ReferenceEquals(EUR.Currency, Currency.Euro));
            Assert.False(Object.ReferenceEquals(Currency.Euro, Currency.Of("EUR")));
        }

        #endregion
    }
}
