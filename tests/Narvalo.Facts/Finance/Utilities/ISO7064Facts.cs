// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;

    using Xunit;

    public static partial class ISO7064Facts
    {
        #region CheckIntegrity()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckIntegrity_ReturnsTrue_UsingInt32Arithmetic(string value)
            => Assert.True(ISO7064.CheckIntegrity(value, false));

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckIntegrity_ReturnsTrue_UsingInt64Arithmetic(string value)
            => Assert.True(ISO7064.CheckIntegrity(value, true));

        #endregion
    }
}
