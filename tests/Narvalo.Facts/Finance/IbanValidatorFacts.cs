// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Xunit;

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class IbanValidatorFacts
    {
        #region CheckIntegrity()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckIntegrity_ReturnsTrue_UsingInt32Arithmetic(string value)
            => Assert.True(IbanValidator.CheckIntegrity(value, false));

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void CheckIntegrity_ReturnsTrue_UsingInt64Arithmetic(string value)
            => Assert.True(IbanValidator.CheckIntegrity(value, true));

        #endregion
    }

#endif
}
