// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class IbanFacts
    {
        #region TryParse()

        [Fact]
        public static void TryParse_ReturnsNull_ForNull()
            => Assert.False(Iban.TryParse(null).HasValue);

        #endregion

        #region TryParseExact()

        [Theory]
        [MemberData(nameof(ValidLengths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParseExact_Succeeds_ForValidFormat(string value)
            => Assert.True(Iban.TryParseExact(value).HasValue);

        [Fact]
        public static void TryParseExact_ReturnsNull_ForNull()
            => Assert.False(Iban.TryParseExact(null).HasValue);

        [Theory]
        [MemberData(nameof(InvalidLengths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParseExact_ReturnsNull_ForInvalidFormat(string value)
            => Assert.False(Iban.TryParseExact(value).HasValue);

        #endregion

        #region Parse()

        [Fact]
        public static void Parse_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Iban.Parse(null));

        #endregion

        #region ParseExact()

        [Theory]
        [MemberData(nameof(ValidLengths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ParseExact_Succeeds(string value)
            => Iban.ParseExact(value);

        [Fact]
        public static void ParseExact_ThrowsArgumentNullException_ForNull()
            => Assert.Throws<ArgumentNullException>(() => Iban.ParseExact(null));

        [Theory]
        [MemberData(nameof(InvalidLengths), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ParseExact_ThrowsFormatException_ForInvalidFormat(string value)
            => Assert.Throws<FormatException>(() => Iban.ParseExact(value));

        #endregion

        #region Create()

        [Fact]
        public static void Create_ThrowsArgumentNullException_ForNull()
        {
            Assert.Throws<ArgumentNullException>(() => Iban.Create(null, "14", "20041010050500013M02606"));
            Assert.Throws<ArgumentNullException>(() => Iban.Create("FR", null, "20041010050500013M02606"));
            Assert.Throws<ArgumentNullException>(() => Iban.Create("FR", "14", null));
        }

        [Theory]
        [MemberData(nameof(IbanFormatFacts.InvalidCountryCodes), MemberType = typeof(IbanFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidCountryCode(string value)
            => Assert.Throws<ArgumentException>(() => Iban.Create(value, "14", "20041010050500013M02606"));

        [Theory]
        [MemberData(nameof(IbanFormatFacts.InvalidCheckDigits), MemberType = typeof(IbanFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidCheckDigits(string value)
            => Assert.Throws<ArgumentException>(() => Iban.Create("FR", value, "20041010050500013M02606"));

        [Theory]
        [MemberData(nameof(IbanFormatFacts.InvalidBbans), MemberType = typeof(IbanFormatFacts), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentException_ForInvalidBban(string value)
            => Assert.Throws<ArgumentException>(() => Iban.Create("FR", "14", value));

        [Fact]
        public static void Create_DoesNotThrow_ForValidInputs()
            => Iban.Create("FR", "14", "20041010050500013M02606");

        #endregion

        #region ParseCore()

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
        public static void ParseCore_SetCountryCodeCorrectly(string value, string expectedValue)
        {
            var iban = Iban.ParseExact(value);

            Assert.Equal(expectedValue, iban.CountryCode);
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
        public static void ParseCore_SetCheckDigitsCorrectly(string value, string expectedValue)
        {
            var iban = Iban.ParseExact(value);

            Assert.Equal(expectedValue, iban.CheckDigits);
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
        public static void ParseCore_SetBbanCorrectly(string value, string expectedValue)
        {
            var iban = Iban.ParseExact(value);

            Assert.Equal(expectedValue, iban.Bban);
        }

        #endregion

        #region op_Equality()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equality_ReturnsTrue_ForIdenticalValues(string value1, string value2)
        {
            var iban1 = Iban.ParseExact(value1);
            var iban2 = Iban.ParseExact(value2);

            Assert.True(iban1 == iban2);
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equality_ReturnsFalse_ForDistinctValues(string value1, string value2)
        {
            var iban1 = Iban.ParseExact(value1);
            var iban2 = Iban.ParseExact(value2);

            Assert.False(iban1 == iban2);
        }

        #endregion

        #region op_Inequality()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Inequality_ReturnsFalse_ForIdenticalValues(string value1, string value2)
        {
            var iban1 = Iban.ParseExact(value1);
            var iban2 = Iban.ParseExact(value2);

            Assert.False(iban1 != iban2);
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Inequality_ReturnsTrue_ForDistinctValues(string value1, string value2)
        {
            var iban1 = Iban.ParseExact(value1);
            var iban2 = Iban.ParseExact(value2);

            Assert.True(iban1 != iban2);
        }

        #endregion

        #region Equals()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalValues(string value1, string value2)
        {
            var iban1 = Iban.ParseExact(value1);
            var iban2 = Iban.ParseExact(value2);

            Assert.True(iban1.Equals(iban2));
        }

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalValues_AfterBoxing(string value1, string value2)
        {
            var iban1 = Iban.ParseExact(value1);
            object iban2 = Iban.ParseExact(value2);

            Assert.True(iban1.Equals(iban2));
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsFalse_ForDistinctValues(string value1, string value2)
        {
            var iban1 = Iban.ParseExact(value1);
            var iban2 = Iban.ParseExact(value2);

            Assert.False(iban1.Equals(iban2));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsFalse_ForOtherTypes(string value)
        {
            var iban = Iban.ParseExact(value);

            Assert.False(iban.Equals(null));
            Assert.False(iban.Equals(1));
            Assert.False(iban.Equals(value));
            Assert.False(iban.Equals(new Object()));
            Assert.False(iban.Equals(new My.SimpleStruct(1)));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsReflexive(string value)
        {
            var iban = Iban.ParseExact(value);

            Assert.True(iban.Equals(iban));
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsAbelian(string value1, string value2)
        {
            var iban1a = Iban.ParseExact(value1);
            var iban1b = Iban.ParseExact(value1);
            var iban2 = Iban.ParseExact(value2);

            Assert.Equal(iban1a.Equals(iban1b), iban1b.Equals(iban1a));
            Assert.Equal(iban1a.Equals(iban2), iban2.Equals(iban1a));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsTransitive(string value)
        {
            var iban1 = Iban.ParseExact(value);
            var iban2 = Iban.ParseExact(value);
            var iban3 = Iban.ParseExact(value);

            Assert.Equal(iban1.Equals(iban2) && iban2.Equals(iban3), iban1.Equals(iban3));
        }

        #endregion

        #region GetHashCode()

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void GetHashCode_ReturnsHashCodeValue(string value)
        {
            var iban = Iban.ParseExact(value);

            Assert.Equal(value.GetHashCode(), iban.GetHashCode());
        }

        #endregion

        #region ToString()

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ReturnsValue(string value)
        {
            var iban = Iban.ParseExact(value);

            Assert.Equal(value, iban.ToString());
        }

        #endregion
    }

    public static partial class IbanFacts
    {
        // Sample IBAN from http://www.rbs.co.uk/corporate/international/g0/guide-to-international-business/regulatory-information/iban/iban-example.ashx.
        public static IEnumerable<object[]> SampleValues
        {
            get
            {
                yield return new object[] { "AL47212110090000000235698741" };
                yield return new object[] { "AD1200012030200359100100" };
                yield return new object[] { "AT611904300234573201" };
                yield return new object[] { "AZ21NABZ00000000137010001944" };
                yield return new object[] { "BH67BMAG00001299123456" };
                yield return new object[] { "BE62510007547061" };
                yield return new object[] { "BA391290079401028494" };
                yield return new object[] { "BG80BNBG96611020345678" };
                yield return new object[] { "HR1210010051863000160" };
                yield return new object[] { "CY17002001280000001200527600" };
                yield return new object[] { "CZ6508000000192000145399" };
                yield return new object[] { "DK5000400440116243" };
                yield return new object[] { "EE382200221020145685" };
                yield return new object[] { "FO9754320388899944" };
                yield return new object[] { "FI2112345600000785" };
                yield return new object[] { "FR1420041010050500013M02606" };
                yield return new object[] { "GE29NB0000000101904917" };
                yield return new object[] { "DE89370400440532013000" };
                yield return new object[] { "GI75NWBK000000007099453" };
                yield return new object[] { "GR1601101250000000012300695" };
                yield return new object[] { "L5604449876543210" };
                yield return new object[] { "HU42117730161111101800000000" };
                yield return new object[] { "IS140159260076545510730339" };
                yield return new object[] { "IE29AIBK93115212345678" };
                yield return new object[] { "IL620108000000099999999" };
                yield return new object[] { "IT40S0542811101000000123456" };
                yield return new object[] { "JO94CBJO0010000000000131000302" };
                yield return new object[] { "KW81CBKU0000000000001234560101" };
                yield return new object[] { "LV80BANK0000435195001" };
                yield return new object[] { "LB62099900000001001901229114" };
                yield return new object[] { "LI21088100002324013AA" };
                yield return new object[] { "LT121000011101001000" };
                yield return new object[] { "LU280019400644750000" };
                yield return new object[] { "MK07250120000058984" };
                yield return new object[] { "MT84MALT011000012345MTLCAST001S" };
                yield return new object[] { "MU17BOMM0101101030300200000MUR" };
                yield return new object[] { "MD24AG000225100013104168" };
                yield return new object[] { "MC9320052222100112233M44555" };
                yield return new object[] { "ME25505000012345678951" };
                yield return new object[] { "NL39RABO0300065264" };
                yield return new object[] { "NO9386011117947" };
                yield return new object[] { "PK36SCBL0000001123456702" };
                yield return new object[] { "PL60102010260000042270201111" };
                yield return new object[] { "PT50000201231234567890154" };
                yield return new object[] { "QA58DOHB00001234567890ABCDEFG" };
                yield return new object[] { "RO49AAAA1B31007593840000" };
                yield return new object[] { "SM86U0322509800000000270100" };
                yield return new object[] { "SA0380000000608010167519" };
                yield return new object[] { "RS35260005601001611379" };
                yield return new object[] { "SK3112000000198742637541" };
                yield return new object[] { "SI56191000000123438" };
                yield return new object[] { "ES8023100001180000012345" };
                yield return new object[] { "SE3550000000054910000003" };
                yield return new object[] { "CH9300762011623852957" };
                yield return new object[] { "TN5910006035183598478831" };
                yield return new object[] { "TR3300061005197864578413 26" };
                yield return new object[] { "AE070331234567890123456" };
                yield return new object[] { "GB29RBOS60161331926819" };
            }
        }

        public static IEnumerable<object[]> IdenticalValues
        {
            get
            {
                yield return new object[] { "12345678901234", "12345678901234" };
                yield return new object[] { "123456789012345", "123456789012345" };
                yield return new object[] { "1234567890123456", "1234567890123456" };
                yield return new object[] { "12345678901234567", "12345678901234567" };
                yield return new object[] { "123456789012345678", "123456789012345678" };
                yield return new object[] { "1234567890123456789", "1234567890123456789" };
                yield return new object[] { "12345678901234567890", "12345678901234567890" };
                yield return new object[] { "123456789012345678901", "123456789012345678901" };
                yield return new object[] { "1234567890123456789012", "1234567890123456789012" };
                yield return new object[] { "12345678901234567890123", "12345678901234567890123" };
                yield return new object[] { "123456789012345678901234", "123456789012345678901234" };
                yield return new object[] { "1234567890123456789012345", "1234567890123456789012345" };
                yield return new object[] { "12345678901234567890123456", "12345678901234567890123456" };
                yield return new object[] { "123456789012345678901234567", "123456789012345678901234567" };
                yield return new object[] { "1234567890123456789012345678", "1234567890123456789012345678" };
                yield return new object[] { "12345678901234567890123456789", "12345678901234567890123456789" };
                yield return new object[] { "123456789012345678901234567890", "123456789012345678901234567890" };
                yield return new object[] { "1234567890123456789012345678901", "1234567890123456789012345678901" };
                yield return new object[] { "12345678901234567890123456789012", "12345678901234567890123456789012" };
                yield return new object[] { "123456789012345678901234567890123", "123456789012345678901234567890123" };
                yield return new object[] { "1234567890123456789012345678901234", "1234567890123456789012345678901234" };
            }
        }

        public static IEnumerable<object[]> DistinctValues
        {
            get
            {
                yield return new object[] { "12345678901234", "123456789012345" };
                yield return new object[] { "123456789012345", "1234567890123456" };
                yield return new object[] { "1234567890123456", "12345678901234567" };
                yield return new object[] { "12345678901234567", "123456789012345678" };
                yield return new object[] { "123456789012345678", "1234567890123456789" };
                yield return new object[] { "1234567890123456789", "12345678901234567890" };
                yield return new object[] { "12345678901234567890", "123456789012345678901" };
                yield return new object[] { "123456789012345678901", "1234567890123456789012" };
                yield return new object[] { "1234567890123456789012", "12345678901234567890123" };
                yield return new object[] { "12345678901234567890123", "123456789012345678901234" };
                yield return new object[] { "123456789012345678901234", "1234567890123456789012345" };
                yield return new object[] { "1234567890123456789012345", "12345678901234567890123456" };
                yield return new object[] { "12345678901234567890123456", "123456789012345678901234567" };
                yield return new object[] { "123456789012345678901234567", "1234567890123456789012345678" };
                yield return new object[] { "1234567890123456789012345678", "12345678901234567890123456789" };
                yield return new object[] { "12345678901234567890123456789", "123456789012345678901234567890" };
                yield return new object[] { "123456789012345678901234567890", "1234567890123456789012345678901" };
                yield return new object[] { "1234567890123456789012345678901", "12345678901234567890123456789012" };
                yield return new object[] { "12345678901234567890123456789012", "123456789012345678901234567890123" };
                yield return new object[] { "123456789012345678901234567890123", "1234567890123456789012345678901234" };
                yield return new object[] { "1234567890123456789012345678901234", "12345678901234" };
            }
        }

        public static IEnumerable<object[]> ValidLengths
        {
            get
            {
                yield return new object[] { "12345678901234" };
                yield return new object[] { "123456789012345" };
                yield return new object[] { "1234567890123456" };
                yield return new object[] { "12345678901234567" };
                yield return new object[] { "123456789012345678" };
                yield return new object[] { "1234567890123456789" };
                yield return new object[] { "12345678901234567890" };
                yield return new object[] { "123456789012345678901" };
                yield return new object[] { "1234567890123456789012" };
                yield return new object[] { "12345678901234567890123" };
                yield return new object[] { "123456789012345678901234" };
                yield return new object[] { "1234567890123456789012345" };
                yield return new object[] { "12345678901234567890123456" };
                yield return new object[] { "123456789012345678901234567" };
                yield return new object[] { "1234567890123456789012345678" };
                yield return new object[] { "12345678901234567890123456789" };
                yield return new object[] { "123456789012345678901234567890" };
                yield return new object[] { "1234567890123456789012345678901" };
                yield return new object[] { "12345678901234567890123456789012" };
                yield return new object[] { "123456789012345678901234567890123" };
                yield return new object[] { "1234567890123456789012345678901234" };
            }
        }

        public static IEnumerable<object[]> InvalidLengths
        {
            get
            {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "12" };
                yield return new object[] { "123" };
                yield return new object[] { "1234" };
                yield return new object[] { "12345" };
                yield return new object[] { "123456" };
                yield return new object[] { "1234567" };
                yield return new object[] { "12345678" };
                yield return new object[] { "123456789" };
                yield return new object[] { "1234567890" };
                yield return new object[] { "12345678901" };
                yield return new object[] { "123456789012" };
                yield return new object[] { "1234567890123" };
                yield return new object[] { "12345678901234567890123456789012345" };
            }
        }

    }
}
