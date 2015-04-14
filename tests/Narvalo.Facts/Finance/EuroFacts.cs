// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using Narvalo.Finance.Currencies;
    using Xunit;

    public static class EuroFacts
    {
        #region op_Equality()

        [Fact]
        public static void Equality_ReturnsTrue_WhenComparingAllEuroInstances()
        {
            // Act & Assert
            Assert.True(EUR.Currency.Equals(Currency.Of("EUR")));
            Assert.True(EUR.Currency.Equals(Euro.Currency));

            Assert.True(Euro.Currency.Equals(Currency.Of("EUR")));
            Assert.True(Euro.Currency.Equals(EUR.Currency));

            Assert.True(EUR.Currency == Currency.Of("EUR"));
            Assert.True(EUR.Currency == Euro.Currency);
            Assert.True(Euro.Currency == Currency.Of("EUR"));
        }

        #endregion
    }
}
