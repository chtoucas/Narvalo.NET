// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;

    using Xunit;

    public static partial class CheckDigitsFacts
    {
        #region CheckIntegrity()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckIntegrity_ReturnsTrue_UsingInt32Arithmetic(string value)
            => Assert.True(CheckDigits.CheckIntegrity(value, false));

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckIntegrity_ReturnsTrue_UsingInt64Arithmetic(string value)
            => Assert.True(CheckDigits.CheckIntegrity(value, true));

        #endregion
    }
}
