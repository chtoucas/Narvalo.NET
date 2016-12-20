// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
#if !NO_INTERNALS_VISIBLE_TO

    using System;

    using Xunit;

    using static Narvalo.Finance.Iban;

    public static partial class IbanPartsFacts
    {
        #region Create()

        [Theory]
        [InlineData("12############", "12")]
        [InlineData("12#############", "12")]
        [InlineData("12##############", "12")]
        [InlineData("12###############", "12")]
        [InlineData("12################", "12")]
        [InlineData("12#################", "12")]
        [InlineData("12##################", "12")]
        [InlineData("12###################", "12")]
        [InlineData("12####################", "12")]
        [InlineData("12#####################", "12")]
        [InlineData("12######################", "12")]
        [InlineData("12#######################", "12")]
        [InlineData("12########################", "12")]
        [InlineData("12#########################", "12")]
        [InlineData("12##########################", "12")]
        [InlineData("12###########################", "12")]
        [InlineData("12############################", "12")]
        [InlineData("12#############################", "12")]
        [InlineData("12##############################", "12")]
        [InlineData("12###############################", "12")]
        [InlineData("12################################", "12")]
        [CLSCompliant(false)]
        public static void GetParts_SetCountryCodeCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.Create(value);

            Assert.Equal(expectedValue, parts.CountryCode);
        }

        [Theory]
        [InlineData("##34##########", "34")]
        [InlineData("##34###########", "34")]
        [InlineData("##34############", "34")]
        [InlineData("##34#############", "34")]
        [InlineData("##34##############", "34")]
        [InlineData("##34###############", "34")]
        [InlineData("##34################", "34")]
        [InlineData("##34#################", "34")]
        [InlineData("##34##################", "34")]
        [InlineData("##34###################", "34")]
        [InlineData("##34####################", "34")]
        [InlineData("##34#####################", "34")]
        [InlineData("##34######################", "34")]
        [InlineData("##34#######################", "34")]
        [InlineData("##34########################", "34")]
        [InlineData("##34#########################", "34")]
        [InlineData("##34##########################", "34")]
        [InlineData("##34###########################", "34")]
        [InlineData("##34############################", "34")]
        [InlineData("##34#############################", "34")]
        [InlineData("##34##############################", "34")]
        [CLSCompliant(false)]
        public static void GetParts_SetCheckDigitsCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.Create(value);

            Assert.Equal(expectedValue, parts.CheckDigits);
        }

        [Theory]
        [InlineData("####5678901234", "5678901234")]
        [InlineData("####56789012345", "56789012345")]
        [InlineData("####567890123456", "567890123456")]
        [InlineData("####5678901234567", "5678901234567")]
        [InlineData("####56789012345678", "56789012345678")]
        [InlineData("####567890123456789", "567890123456789")]
        [InlineData("####5678901234567890", "5678901234567890")]
        [InlineData("####56789012345678901", "56789012345678901")]
        [InlineData("####567890123456789012", "567890123456789012")]
        [InlineData("####5678901234567890123", "5678901234567890123")]
        [InlineData("####56789012345678901234", "56789012345678901234")]
        [InlineData("####567890123456789012345", "567890123456789012345")]
        [InlineData("####5678901234567890123456", "5678901234567890123456")]
        [InlineData("####56789012345678901234567", "56789012345678901234567")]
        [InlineData("####567890123456789012345678", "567890123456789012345678")]
        [InlineData("####5678901234567890123456789", "5678901234567890123456789")]
        [InlineData("####56789012345678901234567890", "56789012345678901234567890")]
        [InlineData("####567890123456789012345678901", "567890123456789012345678901")]
        [InlineData("####5678901234567890123456789012", "5678901234567890123456789012")]
        [InlineData("####56789012345678901234567890123", "56789012345678901234567890123")]
        [InlineData("####567890123456789012345678901234", "567890123456789012345678901234")]
        [CLSCompliant(false)]
        public static void GetParts_SetBbanCorrectly(string value, string expectedValue)
        {
            var parts = IbanParts.Create(value);

            Assert.Equal(expectedValue, parts.Bban);
        }

        #endregion
    }

#endif
}
