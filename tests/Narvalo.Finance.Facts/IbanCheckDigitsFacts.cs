// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Xunit;

    public static partial class IbanCheckDigitsFacts
    {
        #region Compute()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Compute_Succeeds_UsingInt32Arithmetic(string value)
        {
            var iban = IbanParts.Parse(value).Value;

            Assert.Equal(iban.CheckDigits, IbanCheckDigits.Compute(iban.CountryCode, iban.Bban, false));
        }

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Compute_Succeeds_UsingInt64Arithmetic(string value)
        {
            var iban = IbanParts.Parse(value).Value;

            Assert.Equal(iban.CheckDigits, IbanCheckDigits.Compute(iban.CountryCode, iban.Bban, true));
        }

        #endregion

        #region Verify()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Verify_ReturnsTrue_UsingInt32Arithmetic(string value)
            => Assert.True(IbanCheckDigits.Verify(value, false));

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Verify_ReturnsTrue_UsingInt64Arithmetic(string value)
            => Assert.True(IbanCheckDigits.Verify(value, true));

        #endregion

        #region ComputeInt32Checksum()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ComputeInt32Checksum_ReturnsOne(string value)
            => Assert.Equal(1, IbanCheckDigits.ComputeInt32Checksum(value));

        #endregion

        #region ComputeInt64Checksum()

        [Theory]
        [MemberData(nameof(IbanFacts.SampleValues), MemberType = typeof(IbanFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ComputeInt64Checksum_ReturnsOne(string value)
            => Assert.Equal(1, IbanCheckDigits.ComputeInt64Checksum(value));

        #endregion
    }
}
