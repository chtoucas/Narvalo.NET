// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Xunit;

    public static partial class CurrencyFacts
    {
        #region Of()

        [Fact]
        public static void Of_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Currency.Of(null));

        [Fact]
        public static void Of_ThrowsCurrencyNotFoundException_ForUnknownCode()
        {
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of(String.Empty));
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of("A"));
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of("AA"));
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of("AAA"));
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of("AAAA"));
        }

        #endregion

        #region OfRegion()

        [Fact]
        public static void OfRegion_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Currency.OfRegion(null));

        [Fact]
        public static void OfRegion_ReturnsNotNull_ForCurrentRegion()
            => Assert.NotNull(Currency.OfRegion(RegionInfo.CurrentRegion));

        [Theory]
        [InlineData("BE")]
        [InlineData("fr-BE")]
        [InlineData("nl-BE")]
        [InlineData("US")]
        [InlineData("en-US")]
        [InlineData("es-US")]
        [CLSCompliant(false)]
        public static void OfRegion_ReturnsNotNull_ForRegion(string name)
        {
            var ri = new RegionInfo(name);

            Assert.NotNull(Currency.OfRegion(ri));
        }

        #endregion

        #region op_Equality()

        [Fact]
        public static void Equality_ReturnsFalse_ForNullOnOneSide()
        {
            var currency = Currency.Of("EUR");

            Assert.False(currency == null);
            Assert.False(null == currency);
        }

        [Fact]
        public static void Equality_ReturnsTrue_ForNullOnBothSides()
        {
            Currency currency1 = null;
            Currency currency2 = null;

            Assert.False(currency1 == currency2);
        }

        #endregion

        #region Equals()

        [Fact]
        public static void Equals_ReturnsTrue_AfterBoxing()
        {
            var currency1 = Currency.Of("EUR");
            object currency2 = Currency.Of("EUR");

            Assert.False(currency1.Equals(currency2));
        }

        [Fact]
        public static void Equals_ReturnsFalse_ForNull()
        {
            var currency = Currency.Of("EUR");

            Assert.False(currency.Equals(null));
        }

        [Fact]
        public static void Equals_ReturnsFalse_ForNull_AfterBoxing()
        {
            var currency = Currency.Of("EUR");
            object other = null;

            Assert.False(currency.Equals(other));
        }

        [Fact]
        public static void Equals_ReturnsFalse_ForOtherTypes()
        {
            var currency = Currency.Of("EUR");

            Assert.False(currency.Equals(1));
            Assert.False(currency.Equals("EUR"));
            Assert.False(currency.Equals(new Object()));
            Assert.False(currency.Equals(new My.SimpleStruct(1)));
            Assert.False(currency.Equals(new My.SimpleValue { Value = "Whatever" }));
        }

        [Fact]
        public static void Equals_ReturnsFalse_ForDifferentCurrencies()
        {
            var currency1 = Currency.Of("EUR");
            var currency2 = Currency.Of("XPT");

            Assert.False(currency1.Equals(currency2));
        }

        [Fact]
        public static void Equals_FollowsStructuralEqualityRules()
        {
            var currency1 = Currency.Of("EUR");
            var currency2 = Currency.Of("EUR");

            Assert.True(currency1.Equals(currency2));
        }

        #endregion

        [Theory]
        [MemberData(nameof(Aliases), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Aliases_AreNotNull(Currency value) => Assert.NotNull(value);
    }

    public static partial class CurrencyFacts
    {
        public static IEnumerable<object[]> Aliases
        {
            get
            {
                yield return new object[] { Currency.None };
                yield return new object[] { Currency.Test };
                yield return new object[] { Currency.Euro };
                yield return new object[] { Currency.PoundSterling };
                yield return new object[] { Currency.SwissFranc };
                yield return new object[] { Currency.UnitedStatesDollar };
                yield return new object[] { Currency.Yen };
                yield return new object[] { Currency.Gold };
                yield return new object[] { Currency.Palladium };
                yield return new object[] { Currency.Platinum };
                yield return new object[] { Currency.Silver };
            }
        }
    }
}
