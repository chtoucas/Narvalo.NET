// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System;

    using Xunit;

    public static partial class BuiltInCurrencyFacts
    {
        #region Unit

        [Theory]
        [MemberData(nameof(AllUnits), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Unit_ReturnsNonNull(object currency) => Assert.NotNull(currency);

        [Fact]
        public static void Unit_ReturnsSingleton() => Assert.True(ReferenceEquals(EUR.Unit, EUR.Unit));

        #endregion

        #region ToString()

        [Theory]
        [MemberData(nameof(AllUnits), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ReturnsNotNull(object currency) => Assert.NotNull(currency.ToString());

        #endregion
    }
}
