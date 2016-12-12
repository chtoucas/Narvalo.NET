// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Xunit;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = "[Intentionally] Testing all currencies in a row.")]
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "[Intentionally] Testing all currencies in a row.")]
    public static partial class CurrencyFacts
    {
        #region Of()

        [Fact]
        public static void Of_ThrowsArgumentNullException_ForNullCode()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Currency.Of(null));
        }

        [Fact]
        public static void Of_ThrowsCurrencyNotFoundException_ForUnknownCode()
        {
            // Act & Assert
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of(String.Empty));
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of("A"));
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of("AA"));
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of("AAA"));
            Assert.Throws<CurrencyNotFoundException>(() => Currency.Of("AAAA"));
        }

        #endregion

        [Fact]
        public static void Aliases_AreNotNull()
        {
            // Act & Assert
            Assert.NotNull(Currency.None);
            Assert.NotNull(Currency.Test);
            Assert.NotNull(Currency.Euro);
            Assert.NotNull(Currency.PoundSterling);
            Assert.NotNull(Currency.SwissFranc);
            Assert.NotNull(Currency.UnitedStatesDollar);
            Assert.NotNull(Currency.Yen);
            Assert.NotNull(Currency.Gold);
            Assert.NotNull(Currency.Palladium);
            Assert.NotNull(Currency.Platinum);
            Assert.NotNull(Currency.Silver);
        }
    }
}
