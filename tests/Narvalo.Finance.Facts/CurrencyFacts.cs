// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Globalization;

    using Xunit;

    public static partial class CurrencyFacts
    {
        #region Of()

        [Fact]
        public static void Of_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Currency.Of(null));

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("AA")]
        [InlineData("AAA")]
        [InlineData("AAAA")]
        [CLSCompliant(false)]
        public static void Of_ThrowsCurrencyNotFoundException_ForUnknownCode(string value)
            => Assert.Throws<CurrencyNotFoundException>(() => Currency.Of(value));

        #endregion

        #region OfRegion()

        [Fact]
        public static void OfRegion_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Currency.OfRegion(null));

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

        #region OfCurrentRegion()

        [Fact]
        public static void OfCurrentRegion_ReturnsNotNull()
            => Assert.NotNull(Currency.OfCurrentRegion());

        #endregion

        #region OfCulture()

        [Fact]
        public static void OfCulture_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Currency.OfCulture(null));

        [Fact]
        public static void OfCulture_ThrowsArgumentException_ForNeutralCulture()
        {
            var ci = new CultureInfo("fr");

            Assert.Throws<ArgumentException>(() => Currency.OfCulture(ci));
        }

        [Theory]
        [InlineData("fr-BE")]
        [InlineData("nl-BE")]
        [InlineData("en-US")]
        [InlineData("es-US")]
        [CLSCompliant(false)]
        public static void OfCulture_ReturnsNotNull_ForCulture(string name)
        {
            var ci = new CultureInfo(name);

            Assert.NotNull(Currency.OfCulture(ci));
        }

        #endregion

        #region OfCurrentCulture()

        [Fact]
        public static void OfCurrentCulture_ReturnsNotNull()
            => Assert.NotNull(Currency.OfCurrentCulture());

        #endregion

        #region RegisterCurrency()

        [Fact]
        public static void RegisterCurrency_ReturnsFalse_ForExistingCode()
            => Assert.False(Currency.RegisterCurrency("EUR"));

        [Fact]
        public static void RegisterCurrency_ReturnsTrue_ForNonExistingCode()
            => Assert.True(Currency.RegisterCurrency("NEW"));

        #endregion

        #region op_Equality()

        [Theory(DisplayName = "op_Equality() follows structural equality rules.")]
        [MemberData(nameof(AllCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equality_ReturnsTrue_ForIdenticalCodes(string code)
        {
            var currency1 = Currency.Of(code);
            var currency2 = Currency.Of(code);

            Assert.True(currency1 == currency2);
        }

        [Fact]
        public static void Equality_ReturnsFalse_ForDistinctCodes()
        {
            var currency1 = Currency.Of("EUR");
            var currency2 = Currency.Of("JPY");

            Assert.False(currency1 == currency2);
        }

        //[Fact]
        //public static void Equality_ReturnsFalse_ForNullOnOnlyOneSide()
        //{
        //    var currency = Currency.Of("EUR");

        //    Assert.False(currency == null);
        //    Assert.False(null == currency);
        //}

        //[Fact]
        //public static void Equality_ReturnsTrue_ForNullOnBothSides()
        //{
        //    Currency currency1 = null;
        //    Currency currency2 = null;

        //    Assert.True(currency1 == currency2);
        //}

        #endregion

        #region op_Inequality()

        [Fact]
        public static void Inequality_ReturnsFalse_ForIdenticalCodes()
        {
            var currency1 = Currency.Of("EUR");
            var currency2 = Currency.Of("EUR");

            Assert.False(currency1 != currency2);
        }

        [Fact]
        public static void Inequality_ReturnsTrue_ForDistinctCodes()
        {
            var currency1 = Currency.Of("EUR");
            var currency2 = Currency.Of("JPY");

            Assert.True(currency1 != currency2);
        }

        //[Fact]
        //public static void Inequality_ReturnsTrue_ForNullOnOnlyOneSide()
        //{
        //    var currency = Currency.Of("EUR");

        //    Assert.True(currency != null);
        //    Assert.True(null != currency);
        //}

        //[Fact]
        //public static void Inequality_ReturnsFalse_ForNullOnBothSides()
        //{
        //    Currency currency1 = null;
        //    Currency currency2 = null;

        //    Assert.False(currency1 != currency2);
        //}

        #endregion

        #region Equals

        //[Fact]
        //public static void Equals_ReturnsFalse_ForNull()
        //{
        //    var currency = Currency.Of("EUR");
        //    Currency other = null;

        //    Assert.False(currency.Equals(other));
        //}

        [Theory(DisplayName = "Equals() follows structural equality rules.")]
        [MemberData(nameof(AllCodes), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalCodes(string code)
        {
            var currency1 = Currency.Of(code);
            var currency2 = Currency.Of(code);

            Assert.True(currency1.Equals(currency2));
        }

        [Fact]
        public static void Equals_ReturnsFalse_ForDistinctCodes()
        {
            var currency1 = Currency.Of("EUR");
            var currency2 = Currency.Of("XPT");

            Assert.False(currency1.Equals(currency2));
        }

        //[Fact]
        //public static void Equals_ReturnsFalse_ForNull_AfterConversionToRootObject()
        //{
        //    var currency = Currency.Of("EUR");
        //    object other = null;

        //    Assert.False(currency.Equals(other));
        //}

        //[Fact]
        //public static void Equals_ReturnsTrue_ForIdenticalCodes_AfterConversionToRootObject()
        //{
        //    var currency1 = Currency.Of("EUR");
        //    object currency2 = currency1;

        //    Assert.True(currency1.Equals(currency2));
        //}

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

        //[Fact]
        //public static void Equals_ReturnsFalse_ForDistinctCodes_AfterConversionToRootObject()
        //{
        //    var currency1 = Currency.Of("EUR");
        //    object currency2 = Currency.Of("XPT");

        //    Assert.False(currency1.Equals(currency2));
        //}

        [Fact]
        public static void Equals_IsReflexive()
        {
            var currency = Currency.Of("EUR");

            Assert.True(currency.Equals(currency));
        }

        [Fact]
        public static void Equals_IsAbelian()
        {
            var currency1a = Currency.Of("EUR");
            var currency1b = Currency.Of("EUR");
            var currency2 = Currency.Of("JPY");

            Assert.Equal(currency1a.Equals(currency1b), currency1b.Equals(currency1a));
            Assert.Equal(currency1a.Equals(currency2), currency2.Equals(currency1a));
        }

        #endregion

        #region GetHashCode()

        [Theory]
        [InlineData("EUR")]
        [InlineData("USD")]
        [InlineData("JPY")]
        [InlineData("XAU")]
        [CLSCompliant(false)]
        public static void GetHashCode_ReturnsHashCodeValue(string value)
        {
            var currency = Currency.Of(value);

            Assert.Equal(value.GetHashCode(), currency.GetHashCode());
        }

        #endregion
    }
}
