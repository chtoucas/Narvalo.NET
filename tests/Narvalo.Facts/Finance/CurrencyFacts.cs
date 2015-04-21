// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Xunit;

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
            Assert.True(Currency.None != null);
            Assert.True(Currency.Test != null);
            Assert.True(Currency.Euro != null);
            Assert.True(Currency.PoundSterling != null);
            Assert.True(Currency.SwissFranc != null);
            Assert.True(Currency.UnitedStatesDollar != null);
            Assert.True(Currency.Yen != null);
            Assert.True(Currency.Gold != null);
            Assert.True(Currency.Palladium != null);
            Assert.True(Currency.Platinum != null);
            Assert.True(Currency.Silver != null);
        }
    }
}
