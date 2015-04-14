// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Currencies;
    using Xunit;

    public static class MoneyFacts
    {
        [Fact]
        public static void Test1()
        {
            // Arrange
            var amount1 = new Money<EUR>(1m);
            var amount2 = new Money<EUR>(1m);

            // Act & Assert
            Assert.True(amount1.Equals(amount2));
            Assert.True(amount1 == amount2);
        }

        [Fact]
        public static void Test2()
        {
            // Arrange
            var amount1 = new Money<EUR>(1m);
            var amount2 = new Money(1m, Euro.Currency);

            // Act & Assert
            Assert.True(amount1.Equals(amount2));
            Assert.True(amount1 == amount2);
        }

        [Fact]
        public static void Test3()
        {
            // Arrange
            var money = new Money(1m, Euro.Currency);

            // Act
            Money money1 = new Money<EUR>(1m);
            Money<EUR> money2 = (Money<EUR>)money;

            Assert.Throws<InvalidCastException>(() => (Money<CHF>)money);
        }
    }
}
