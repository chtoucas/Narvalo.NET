// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static class CheckDigitFacts
    {
        public static IEnumerable<object[]> Compute1AlphaTestData
        {
            get
            {
                yield return new object[] { "ABCDEFG", "S" };
            }
        }

        public static IEnumerable<object[]> Compute1AlphanumTestData
        {
            get
            {
                yield return new object[] { "ABCD1234", "N" };
            }
        }

        public static IEnumerable<object[]> Compute1NumTestData
        {
            get
            {
                yield return new object[] { "0", "2" };
                yield return new object[] { "1", "9" };
                yield return new object[] { "2", "7" };
                yield return new object[] { "3", "5" };
                yield return new object[] { "4", "3" };
                yield return new object[] { "5", "1" };
                yield return new object[] { "6", "0" };
                yield return new object[] { "7", "8" };
                yield return new object[] { "8", "6" };
                yield return new object[] { "9", "4" };
                yield return new object[] { "10", "7" };
                yield return new object[] { "11", "5" };
                yield return new object[] { "12", "3" };
                yield return new object[] { "13", "1" };
                yield return new object[] { "12345", "0" };
            }
        }

        #region Verify1()

        [Fact]
        public static void Verify1_ThrowsArgumentOutOfRangeException_ForEmptyInput()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify1(String.Empty, CheckDigitsRange.Alphabetic));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify1(String.Empty, CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify1(String.Empty, CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify1(String.Empty, CheckDigitsRange.Numeric));
        }

        [Fact]
        public static void Verify1_ThrowsArgumentOutOfRangeException_ForInputOfLength1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify1("1", CheckDigitsRange.Alphabetic));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify1("1", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify1("1", CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify1("1", CheckDigitsRange.Numeric));
        }

        [Fact]
        public static void Verify1_ReturnsTrue_ForValidInput()
        {
            Assert.True(CheckDigits.Verify1("ABCDEFGS", CheckDigitsRange.Alphabetic));
            Assert.True(CheckDigits.Verify1("ABCD1234N", CheckDigitsRange.Alphanumeric));
            Assert.True(CheckDigits.Verify1("123450", CheckDigitsRange.Numeric));
        }

        #endregion

        #region Verify2()

        [Fact]
        public static void Verify2_ThrowsArgumentOutOfRangeException_ForEmptyInput()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2(String.Empty, CheckDigitsRange.Alphabetic));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2(String.Empty, CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2(String.Empty, CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2(String.Empty, CheckDigitsRange.Numeric));
        }

        [Fact]
        public static void Verify2_ThrowsArgumentOutOfRangeException_ForInputOfLength1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2("1", CheckDigitsRange.Alphabetic));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2("1", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2("1", CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2("1", CheckDigitsRange.Numeric));
        }

        [Fact]
        public static void Verify2_ThrowsArgumentOutOfRangeException_ForInputOfLength2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2("12", CheckDigitsRange.Alphabetic));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2("12", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2("12", CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentOutOfRangeException>(() => CheckDigits.Verify2("12", CheckDigitsRange.Numeric));
        }

        [Fact]
        public static void Verify2_ReturnsTrue_ForValidInput()
        {
            Assert.True(CheckDigits.Verify2("ABCDEFGAZ", CheckDigitsRange.Alphabetic));
            Assert.True(CheckDigits.Verify2("ABCD1234KP", CheckDigitsRange.Alphanumeric));
            Assert.True(CheckDigits.Verify2("1234520", CheckDigitsRange.Numeric));
        }

        #endregion

        #region Compute1()

        [Fact]
        public static void Compute1_ThrowsArgumentException_ForInvalidInput()
        {
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute1("A1", CheckDigitsRange.Alphabetic));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute1("A*", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute1("Aa", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute1("Aé", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute1("AG", CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute1("1f", CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute1("1A", CheckDigitsRange.Numeric));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute1("1a", CheckDigitsRange.Numeric));
        }

        [Theory]
        [MemberData("Compute1AlphaTestData")]
        [CLSCompliant(false)]
        public static void Compute1_ReturnsExpectedString_ForAlphaInput(string value, string expectedValue)
        {
            Assert.Equal(expectedValue, CheckDigits.Compute1(value, CheckDigitsRange.Alphabetic));
        }

        [Theory]
        [MemberData("Compute1AlphanumTestData")]
        [CLSCompliant(false)]
        public static void Compute1_ReturnsExpectedString_ForAlphanumInput(string value, string expectedValue)
        {
            Assert.Equal(expectedValue, CheckDigits.Compute1(value, CheckDigitsRange.Alphanumeric));
        }

        [Theory]
        [MemberData("Compute1NumTestData")]
        [CLSCompliant(false)]
        public static void Compute1_ReturnsExpectedString_ForNumInput(string value, string expectedValue)
        {
            Assert.Equal(expectedValue, CheckDigits.Compute1(value, CheckDigitsRange.Numeric));
        }

        #endregion

        #region Compute2()

        [Fact]
        public static void Compute2_ThrowsArgumentException_ForInvalidInput()
        {
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute2("AA1", CheckDigitsRange.Alphabetic));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute2("AA*", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute2("AAa", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute2("AAé", CheckDigitsRange.Alphanumeric));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute2("AAG", CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute2("A1f", CheckDigitsRange.Hexadecimal));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute2("11A", CheckDigitsRange.Numeric));
            Assert.Throws<ArgumentException>(() => CheckDigits.Compute2("11a", CheckDigitsRange.Numeric));
        }

        [Fact]
        public static void Compute2_ForValidInput()
        {
            Assert.Equal("AZ", CheckDigits.Compute2("ABCDEFG", CheckDigitsRange.Alphabetic));
            Assert.Equal("KP", CheckDigits.Compute2("ABCD1234", CheckDigitsRange.Alphanumeric));
            Assert.Equal("20", CheckDigits.Compute2("12345", CheckDigitsRange.Numeric));
        }

        #endregion
    }
}
