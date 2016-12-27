// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Currencies;
    using Xunit;

    public static partial class MoneyFacts
    {
        [Fact]
        public static void Test1()
        {
            var amount1 = new Money<EUR>(1m);
            var amount2 = new Money<EUR>(1m);

            Assert.True(amount1.Equals(amount2));
            Assert.True(amount1 == amount2);
        }

        //[Fact]
        //public static void Test2()
        //{
        //    // Arrange
        //    var amount1 = new Money<EUR>(1m);
        //    var amount2 = new Money(1m, CurrencyUnit.Euro);

        //    // Act & Assert
        //    Assert.False(amount1.Equals(amount2));
        //    ////Assert.True(amount1 == amount2);
        //}

        //[Fact]
        //public static void Test3()
        //{
        //    // Arrange
        //    var money = new Money(1m, CurrencyUnit.Euro);

        //    // Act
        //    ////Money money1 = new Money<EUR>(1m);
        //    ////Money<EUR> money2 = (Money<EUR>)money;
        //    ////Money money3 = new Money<EUR>(1m);

        //    ////Assert.True(money1.Currency == EUR.Currency);
        //    ////Assert.True(Object.ReferenceEquals(money1.Currency, EUR.Currency));
        //    ////Assert.True(Object.ReferenceEquals(money1.Currency, money3.Currency));

        //    Assert.Throws<InvalidCastException>(() => (Money<CHF>)money);
        //}
    }
}
