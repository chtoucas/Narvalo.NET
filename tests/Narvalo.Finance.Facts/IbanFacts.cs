// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class IbanFacts
    {
        #region Parse()

        [Fact]
        public static void Parse_ReturnsNull_ForNull()
            => Assert.False(Iban.Parse(null).HasValue);

        [Theory]
        [InlineData("     AL47 2121 1009 0000 0002 3569 8741")]
        [InlineData("AL47 2121 1009 0000 0002 3569 8741     ")]
        [InlineData("IBAN AL47 2121 1009 0000 0002 3569 8741")]
        [InlineData("     IBAN AL47 2121 1009 0000 0002 3569 8741")]
        [CLSCompliant(false)]
        public static void Parse_Succeeds_(string value)
            => Assert.True(Iban.Parse(value, IbanStyles.Any).HasValue);

        [Theory]
        [InlineData("al47212110090000000235698741")]
        [InlineData("Al47212110090000000235698741")]
        [InlineData("mt84malt011000012345mtlcast001s")]
        [CLSCompliant(false)]
        public static void Parse_Succeeds_Lowercase(string value)
            => Assert.True(Iban.Parse(value, IbanStyles.AllowLowercaseLetter).HasValue);

        #endregion

        #region TryParse()

        [Fact]
        public static void TryParse_ReturnsFailure_ForNull()
            => Assert.False(Iban.TryParse(null).Success);

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void TryParse_Succeeds_ForValidInput(string value)
            => Assert.True(Iban.TryParse(value).Success);

        #endregion

        #region Create()

        [Theory]
        [InlineData(null, "14", "20041010050500013M02606")]
        [InlineData("FR", null, "20041010050500013M02606")]
        [InlineData("FR", "14", null)]
        [CLSCompliant(false)]
        public static void Create_ThrowsArgumentNullException_ForNull(string countryCode, string checkDigits, string bban)
            => Assert.Throws<ArgumentNullException>(() => Iban.Create(countryCode, checkDigits, bban));

        #endregion

        #region op_Equality()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equality_ReturnsTrue_ForIdenticalValues(string value1, string value2)
        {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.True(iban1 == iban2);
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equality_ReturnsFalse_ForDistinctValues(string value1, string value2)
        {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.False(iban1 == iban2);
        }

        #endregion

        #region op_Inequality()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Inequality_ReturnsFalse_ForIdenticalValues(string value1, string value2)
        {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.False(iban1 != iban2);
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Inequality_ReturnsTrue_ForDistinctValues(string value1, string value2)
        {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.True(iban1 != iban2);
        }

        #endregion

        #region Equals()

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalValues(string value1, string value2)
        {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.True(iban1.Equals(iban2));
        }

        [Theory]
        [MemberData(nameof(IdenticalValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsTrue_ForIdenticalValues_AfterBoxing(string value1, string value2)
        {
            var iban1 = ParseFast(value1);
            object iban2 = ParseFast(value2);

            Assert.True(iban1.Equals(iban2));
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsFalse_ForDistinctValues(string value1, string value2)
        {
            var iban1 = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.False(iban1.Equals(iban2));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_ReturnsFalse_ForOtherTypes(string value)
        {
            var iban = ParseFast(value);

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
            var iban = ParseFast(value);

            Assert.True(iban.Equals(iban));
        }

        [Theory]
        [MemberData(nameof(DistinctValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsAbelian(string value1, string value2)
        {
            var iban1a = ParseFast(value1);
            var iban1b = ParseFast(value1);
            var iban2 = ParseFast(value2);

            Assert.Equal(iban1a.Equals(iban1b), iban1b.Equals(iban1a));
            Assert.Equal(iban1a.Equals(iban2), iban2.Equals(iban1a));
        }

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Equals_IsTransitive(string value)
        {
            var iban1 = ParseFast(value);
            var iban2 = ParseFast(value);
            var iban3 = ParseFast(value);

            Assert.Equal(iban1.Equals(iban2) && iban2.Equals(iban3), iban1.Equals(iban3));
        }

        #endregion

        #region GetHashCode()

        [Theory]
        [MemberData(nameof(SampleValues), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void GetHashCode_ReturnsHashCodeValue(string value)
        {
            var result = ParseFast(value).GetHashCode();
            var expected = value.GetHashCode();

            Assert.Equal(expected, result);
        }

        #endregion

        #region ToString()

        [Theory]
        [InlineData(" ")]
        [InlineData("X")]
        [InlineData("XX")]
        [CLSCompliant(false)]
        public static void ToString_ThrowsFormatException_ForInvalidFormat(string value)
            => Assert.Throws<FormatException>(
                () => ParseFast("AL47212110090000000235698741").ToString(value));

        [Theory]
        [MemberData(nameof(FormatSamples), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ForNullFormat(string value, string formattedValue)
        {
            var result = ParseFast(value).ToString(null);

            Assert.Equal(formattedValue, result);
        }

        [Theory]
        [MemberData(nameof(FormatSamples), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ForEmptyFormat(string value, string formattedValue)
        {
            var result = ParseFast(value).ToString(String.Empty);

            Assert.Equal(formattedValue, result);
        }

        [Theory]
        [MemberData(nameof(FormatSamples), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ForDefaultFormat(string value, string formattedValue)
        {
            var result = ParseFast(value).ToString();

            Assert.Equal(formattedValue, result);
        }

        [Theory]
        [MemberData(nameof(FormatSamples), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ForGeneralFormat(string value, string formattedValue)
        {
            var result1 = ParseFast(value).ToString("G");
            var result2 = ParseFast(value).ToString("g");

            Assert.Equal(formattedValue, result1);
            Assert.Equal(formattedValue, result2);
        }

        [Theory]
        [MemberData(nameof(FormatSamples), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ForHumanFormat(string value, string formattedValue)
        {
            var result1 = ParseFast(value).ToString("H");
            var result2 = ParseFast(value).ToString("h");
            var expected = IbanParts.HumanHeader + formattedValue;

            Assert.Equal(expected, result1);
            Assert.Equal(expected, result2);
        }

        [Theory]
        [MemberData(nameof(FormatSamples), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void ToString_ReturnsValue_ForCompactFormat(string value, string formattedValue)
        {
            var result1 = ParseFast(value).ToString("C");
            var result2 = ParseFast(value).ToString("c");

            Assert.Equal(value, result1);
            Assert.Equal(value, result2);
        }

        #endregion
    }

    public static partial class IbanFacts
    {
        private static Iban ParseFast(string value)
            => Iban.Parse(value, IbanStyles.None, IbanValidationLevels.None).Value;

        // Sample IBANs from http://www.rbs.co.uk/corporate/international/g0/guide-to-international-business/regulatory-information/iban/iban-example.ashx.
        // except the last one "GB29 RBOS 6016 1331 9268 19" which is not valid;
        // we use instead http://www.xe.com/ibancalculator/sample/?ibancountry=united-kingdom.
        public static IEnumerable<object[]> SampleDisplayValues
        {
            get
            {
                yield return new object[] { "AL47 2121 1009 0000 0002 3569 8741" };
                yield return new object[] { "AD12 0001 2030 2003 5910 0100" };
                yield return new object[] { "AT61 1904 3002 3457 3201" };
                yield return new object[] { "AZ21 NABZ 0000 0000 1370 1000 1944" };
                yield return new object[] { "BH67 BMAG 0000 1299 1234 56" };
                yield return new object[] { "BE62 5100 0754 7061" };
                yield return new object[] { "BA39 1290 0794 0102 8494" };
                yield return new object[] { "BG80 BNBG 9661 1020 3456 78" };
                yield return new object[] { "HR12 1001 0051 8630 0016 0" };
                yield return new object[] { "CY17 0020 0128 0000 0012 0052 7600" };
                yield return new object[] { "CZ65 0800 0000 1920 0014 5399" };
                yield return new object[] { "DK50 0040 0440 1162 43" };
                yield return new object[] { "EE38 2200 2210 2014 5685" };
                yield return new object[] { "FO97 5432 0388 8999 44" };
                yield return new object[] { "FI21 1234 5600 0007 85" };
                yield return new object[] { "FR14 2004 1010 0505 0001 3M02 606" };
                yield return new object[] { "GE29 NB00 0000 0101 9049 17" };
                yield return new object[] { "DE89 3704 0044 0532 0130 00" };
                yield return new object[] { "GI75 NWBK 0000 0000 7099 453" };
                yield return new object[] { "GR16 0110 1250 0000 0001 2300 695" };
                yield return new object[] { "GL56 0444 9876 5432 10" };
                yield return new object[] { "HU42 1177 3016 1111 1018 0000 0000" };
                yield return new object[] { "IS14 0159 2600 7654 5510 7303 39" };
                yield return new object[] { "IE29 AIBK 9311 5212 3456 78" };
                yield return new object[] { "IL62 0108 0000 0009 9999 999" };
                yield return new object[] { "IT40 S054 2811 1010 0000 0123 456" };
                yield return new object[] { "JO94 CBJO 0010 0000 0000 0131 0003 02" };
                yield return new object[] { "KW81 CBKU 0000 0000 0000 1234 5601 01" };
                yield return new object[] { "LV80 BANK 0000 4351 9500 1" };
                yield return new object[] { "LB62 0999 0000 0001 0019 0122 9114" };
                yield return new object[] { "LI21 0881 0000 2324 013A A" };
                yield return new object[] { "LT12 1000 0111 0100 1000" };
                yield return new object[] { "LU28 0019 4006 4475 0000" };
                yield return new object[] { "MK072 5012 0000 0589 84" };
                yield return new object[] { "MT84 MALT 0110 0001 2345 MTLC AST0 01S" };
                yield return new object[] { "MU17 BOMM 0101 1010 3030 0200 000M UR" };
                yield return new object[] { "MD24 AG00 0225 1000 1310 4168" };
                yield return new object[] { "MC93 2005 2222 1001 1223 3M44 555" };
                yield return new object[] { "ME25 5050 0001 2345 6789 51" };
                yield return new object[] { "NL39 RABO 0300 0652 64" };
                yield return new object[] { "NO93 8601 1117 947" };
                yield return new object[] { "PK36 SCBL 0000 0011 2345 6702" };
                yield return new object[] { "PL60 1020 1026 0000 0422 7020 1111" };
                yield return new object[] { "PT50 0002 0123 1234 5678 9015 4" };
                yield return new object[] { "QA58 DOHB 0000 1234 5678 90AB CDEF G" };
                yield return new object[] { "RO49 AAAA 1B31 0075 9384 0000" };
                yield return new object[] { "SM86 U032 2509 8000 0000 0270 100" };
                yield return new object[] { "SA03 8000 0000 6080 1016 7519" };
                yield return new object[] { "RS35 2600 0560 1001 6113 79" };
                yield return new object[] { "SK31 1200 0000 1987 4263 7541" };
                yield return new object[] { "SI56 1910 0000 0123 438" };
                yield return new object[] { "ES80 2310 0001 1800 0001 2345" };
                yield return new object[] { "SE35 5000 0000 0549 1000 0003" };
                yield return new object[] { "CH93 0076 2011 6238 5295 7" };
                yield return new object[] { "TN59 1000 6035 1835 9847 8831" };
                yield return new object[] { "TR33 0006 1005 1978 6457 8413 26" };
                yield return new object[] { "AE07 0331 2345 6789 0123 456" };
                yield return new object[] { "GB29 NWBK 6016 1331 9268 19" };
            }
        }

        // See comments before SampleDisplayValues.
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
                yield return new object[] { "GL5604449876543210" };
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
                yield return new object[] { "TR330006100519786457841326" };
                yield return new object[] { "AE070331234567890123456" };
                yield return new object[] { "GB29NWBK60161331926819" };
            }
        }

        public static IEnumerable<object[]> IdenticalValues
        {
            get
            {
                yield return new object[] { "FR345678901234", "FR345678901234" };
                yield return new object[] { "FR3456789012345", "FR3456789012345" };
                yield return new object[] { "FR34567890123456", "FR34567890123456" };
                yield return new object[] { "FR345678901234567", "FR345678901234567" };
                yield return new object[] { "FR3456789012345678", "FR3456789012345678" };
                yield return new object[] { "FR34567890123456789", "FR34567890123456789" };
                yield return new object[] { "FR345678901234567890", "FR345678901234567890" };
                yield return new object[] { "FR3456789012345678901", "FR3456789012345678901" };
                yield return new object[] { "FR34567890123456789012", "FR34567890123456789012" };
                yield return new object[] { "FR345678901234567890123", "FR345678901234567890123" };
                yield return new object[] { "FR3456789012345678901234", "FR3456789012345678901234" };
                yield return new object[] { "FR34567890123456789012345", "FR34567890123456789012345" };
                yield return new object[] { "FR345678901234567890123456", "FR345678901234567890123456" };
                yield return new object[] { "FR3456789012345678901234567", "FR3456789012345678901234567" };
                yield return new object[] { "FR34567890123456789012345678", "FR34567890123456789012345678" };
                yield return new object[] { "FR345678901234567890123456789", "FR345678901234567890123456789" };
                yield return new object[] { "FR3456789012345678901234567890", "FR3456789012345678901234567890" };
                yield return new object[] { "FR34567890123456789012345678901", "FR34567890123456789012345678901" };
                yield return new object[] { "FR345678901234567890123456789012", "FR345678901234567890123456789012" };
                yield return new object[] { "FR3456789012345678901234567890123", "FR3456789012345678901234567890123" };
                yield return new object[] { "FR34567890123456789012345678901234", "FR34567890123456789012345678901234" };
            }
        }

        public static IEnumerable<object[]> DistinctValues
        {
            get
            {
                yield return new object[] { "FR345678901234", "FR3456789012345" };
                yield return new object[] { "FR3456789012345", "FR34567890123456" };
                yield return new object[] { "FR34567890123456", "FR345678901234567" };
                yield return new object[] { "FR345678901234567", "FR3456789012345678" };
                yield return new object[] { "FR3456789012345678", "FR34567890123456789" };
                yield return new object[] { "FR34567890123456789", "FR345678901234567890" };
                yield return new object[] { "FR345678901234567890", "FR3456789012345678901" };
                yield return new object[] { "FR3456789012345678901", "FR34567890123456789012" };
                yield return new object[] { "FR34567890123456789012", "FR345678901234567890123" };
                yield return new object[] { "FR345678901234567890123", "FR3456789012345678901234" };
                yield return new object[] { "FR3456789012345678901234", "FR34567890123456789012345" };
                yield return new object[] { "FR34567890123456789012345", "FR345678901234567890123456" };
                yield return new object[] { "FR345678901234567890123456", "FR3456789012345678901234567" };
                yield return new object[] { "FR3456789012345678901234567", "FR34567890123456789012345678" };
                yield return new object[] { "FR34567890123456789012345678", "FR345678901234567890123456789" };
                yield return new object[] { "FR345678901234567890123456789", "FR3456789012345678901234567890" };
                yield return new object[] { "FR3456789012345678901234567890", "FR34567890123456789012345678901" };
                yield return new object[] { "FR34567890123456789012345678901", "FR345678901234567890123456789012" };
                yield return new object[] { "FR345678901234567890123456789012", "FR3456789012345678901234567890123" };
                yield return new object[] { "FR3456789012345678901234567890123", "FR34567890123456789012345678901234" };
                yield return new object[] { "FR34567890123456789012345678901234", "FR345678901234" };
            }
        }

        public static IEnumerable<object[]> FormatSamples
        {
            get
            {
                yield return new object[] { "FR345678901234", "FR34 5678 9012 34" };
                yield return new object[] { "FR3456789012345", "FR34 5678 9012 345" };
                yield return new object[] { "FR34567890123456", "FR34 5678 9012 3456" };
                yield return new object[] { "FR345678901234567", "FR34 5678 9012 3456 7" };
                yield return new object[] { "FR3456789012345678", "FR34 5678 9012 3456 78" };
                yield return new object[] { "FR34567890123456789", "FR34 5678 9012 3456 789" };
                yield return new object[] { "FR345678901234567890", "FR34 5678 9012 3456 7890" };
                yield return new object[] { "FR3456789012345678901", "FR34 5678 9012 3456 7890 1" };
                yield return new object[] { "FR34567890123456789012", "FR34 5678 9012 3456 7890 12" };
                yield return new object[] { "FR345678901234567890123", "FR34 5678 9012 3456 7890 123" };
                yield return new object[] { "FR3456789012345678901234", "FR34 5678 9012 3456 7890 1234" };
                yield return new object[] { "FR34567890123456789012345", "FR34 5678 9012 3456 7890 1234 5" };
                yield return new object[] { "FR345678901234567890123456", "FR34 5678 9012 3456 7890 1234 56" };
                yield return new object[] { "FR3456789012345678901234567", "FR34 5678 9012 3456 7890 1234 567" };
                yield return new object[] { "FR34567890123456789012345678", "FR34 5678 9012 3456 7890 1234 5678" };
                yield return new object[] { "FR345678901234567890123456789", "FR34 5678 9012 3456 7890 1234 5678 9" };
                yield return new object[] { "FR3456789012345678901234567890", "FR34 5678 9012 3456 7890 1234 5678 90" };
                yield return new object[] { "FR34567890123456789012345678901", "FR34 5678 9012 3456 7890 1234 5678 901" };
                yield return new object[] { "FR345678901234567890123456789012", "FR34 5678 9012 3456 7890 1234 5678 9012" };
                yield return new object[] { "FR3456789012345678901234567890123", "FR34 5678 9012 3456 7890 1234 5678 9012 3" };
                yield return new object[] { "FR34567890123456789012345678901234", "FR34 5678 9012 3456 7890 1234 5678 9012 34" };
            }
        }
    }
}
