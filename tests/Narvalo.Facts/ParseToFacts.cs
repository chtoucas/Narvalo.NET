// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class ParseToFacts
    {
        #region Boolean()

        #region BooleanStyles.Default

        [Fact]
        public static void Boolean_ReturnsNull_ForNullString()
        {
            var result = ParseTo.Boolean(null);

            Assert.False(result.HasValue);
        }

        #endregion
        #region BooleanStyles.ZeroOrOne

        [Theory]
        [InlineData("1")]
        [InlineData("1 ")]
        [InlineData(" 1")]
        [InlineData(" 1 ")]
        [CLSCompliant(false)]
        public static void Boolean_ReturnsTrue_ForLiteralOne_ZeroOrOneStyle(string value)
        {
            var result = ParseTo.Boolean(value, BooleanStyles.ZeroOrOne);

            Assert.True(result.HasValue);
            Assert.True(result.Value);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("0 ")]
        [InlineData(" 0")]
        [InlineData(" 0 ")]
        [CLSCompliant(false)]
        public static void Boolean_ReturnsFalse_ForLiteralZero_ZeroOrOneStyle(string value)
        {
            var result = ParseTo.Boolean(value, BooleanStyles.ZeroOrOne);

            Assert.True(result.HasValue);
            Assert.False(result.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("10")]
        [InlineData("10.1")]
        [InlineData("-1")]
        [InlineData("-10")]
        [InlineData("-10.1")]
        [InlineData("a")]
        [InlineData(" a")]
        [InlineData("a ")]
        [InlineData(" a ")]
        [InlineData("Whatever")]
        [InlineData("Whatever ")]
        [InlineData(" Whatever")]
        [InlineData(" Whatever ")]
        [CLSCompliant(false)]
        public static void Boolean_ReturnsNull_ForInvalidInput_ZeroOrOneStyle(string value)
        {
            var result = ParseTo.Boolean(value, BooleanStyles.ZeroOrOne);

            Assert.False(result.HasValue);
        }

        #endregion
        #region BooleanStyles.EmptyOrWhiteSpaceIsFalse

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [CLSCompliant(false)]
        public static void Boolean_ReturnsFalse_ForEmptyString_EmptyOrWhiteSpaceIsFalseStyle(string value)
        {
            var result = ParseTo.Boolean(value, BooleanStyles.EmptyOrWhiteSpaceIsFalse);

            Assert.True(result.HasValue);
            Assert.False(result.Value);
        }

        #endregion
        #region BooleanStyles.Literal

        [Theory]
        [InlineData("true")]
        [InlineData("TrUe")]
        [InlineData(" true")]
        [InlineData(" TrUe")]
        [InlineData("true ")]
        [InlineData("TrUe ")]
        [InlineData(" true ")]
        [InlineData(" TrUe ")]
        [CLSCompliant(false)]
        public static void Boolean_ReturnsTrue_ForLiteralTrue_LiteralStyle(string value)
        {
            var result = ParseTo.Boolean(value, BooleanStyles.Literal);

            Assert.True(result.HasValue);
            Assert.True(result.Value);
        }

        [Theory]
        [InlineData("false")]
        [InlineData("fAlSe")]
        [InlineData("falsE")]
        [InlineData(" false")]
        [InlineData(" fAlSe")]
        [InlineData("false ")]
        [InlineData("fAlSe ")]
        [InlineData(" false ")]
        [InlineData(" fAlSe ")]
        [CLSCompliant(false)]
        public static void Boolean_ReturnsFalse_ForLiteralFalse_LiteralStyle(string value)
        {
            var result = ParseTo.Boolean(value, BooleanStyles.Literal);

            Assert.True(result.HasValue);
            Assert.False(result.Value);
        }

        #endregion

        #endregion

        #region Enum()

        [Fact]
        public static void Enum_ThrowsArgumentException_ForInt32()
            => Assert.Throws<ArgumentException>(() => ParseTo.Enum<int>("Whatever"));

        [Fact]
        public static void Enum_ThrowsArgumentException_ForNonEnumerationStruct()
            => Assert.Throws<ArgumentException>(() => ParseTo.Enum<My.EmptyStruct>("Whatever"));

        [Theory]
        [InlineData("1")]
        [InlineData("1 ")]
        [InlineData(" 1")]
        [InlineData(" 1 ")]
        [InlineData("One")]
        [InlineData("one")]
        [InlineData("oNe")]
        [InlineData("onE")]
        [InlineData("One ")]
        [InlineData("one ")]
        [InlineData(" One")]
        [InlineData(" one")]
        [InlineData(" One ")]
        [InlineData(" one ")]
        [CLSCompliant(false)]
        public static void Enum_Succeeds_ForValidInput(string value)
        {
            var result = ParseTo.Enum<My.Enum012>(value);

            Assert.True(result.HasValue);
            Assert.Equal(My.Enum012.One, result.Value);
        }

        [Theory]
        [InlineData("4")]
        [InlineData("4 ")]
        [InlineData(" 4")]
        [InlineData(" 4 ")]
        [CLSCompliant(false)]
        // Weird but passing any integer value will succeed.
        public static void Enum_Succeeds_ForInvalidIntegerInput(string value)
        {
            var result = ParseTo.Enum<My.Enum012>(value);

            Assert.True(result.HasValue);
            Assert.Equal((My.Enum012)4, result.Value);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("a ")]
        [InlineData(" a")]
        [InlineData(" a ")]
        [InlineData("Whatever")]
        [InlineData("Whatever ")]
        [InlineData(" Whatever")]
        [InlineData(" Whatever ")]
        [CLSCompliant(false)]
        public static void Enum_ReturnsNull_ForInvalidValue(string value)
        {
            var result = ParseTo.Enum<My.Enum012>(value);

            Assert.False(result.HasValue);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("a ")]
        [InlineData(" a")]
        [InlineData(" a ")]
        [InlineData("Whatever")]
        [InlineData("Whatever ")]
        [InlineData(" Whatever")]
        [InlineData(" Whatever ")]
        [CLSCompliant(false)]
        public static void Enum_ReturnsNull_ForInvalidValue_DoNotIgnoreCase(string value)
        {
            var result = ParseTo.Enum<My.Enum012>(value, ignoreCase: false);

            Assert.False(result.HasValue);
        }

        [Theory]
        [InlineData("one")]
        [InlineData("oNe")]
        [InlineData("one ")]
        [InlineData("oNe ")]
        [InlineData(" one")]
        [InlineData(" oNe")]
        [InlineData(" one ")]
        [InlineData(" oNe ")]
        [CLSCompliant(false)]
        public static void Enum_ReturnsNull_ForValidInput_DoNotIgnoreCase(string value)
        {
            var result = ParseTo.Enum<My.Enum012>(value, ignoreCase: false);

            Assert.False(result.HasValue);
        }

        [Theory]
        [InlineData("Alias1")]
        [InlineData("alias1")]
        [InlineData("aliaS1")]
        [InlineData("Alias1 ")]
        [InlineData("alias1 ")]
        [InlineData(" Alias1")]
        [InlineData(" alias1")]
        [InlineData(" Alias1 ")]
        [InlineData(" alias1 ")]
        [CLSCompliant(false)]
        public static void Enum_Succeeds_ForAliasValue(string value)
        {
            var result = ParseTo.Enum<My.Enum012>(value);

            Assert.True(result.HasValue);
            Assert.Equal(My.Enum012.Alias1, result.Value);
        }

        [Theory]
        [InlineData("alias1")]
        [InlineData("aliaS1")]
        [InlineData("alias1 ")]
        [InlineData(" alias1")]
        [InlineData(" alias1 ")]
        [CLSCompliant(false)]
        public static void Enum_ReturnsNull_ForAliasValue_DoNotIgnoreCase(string value)
        {
            var result = ParseTo.Enum<My.Enum012>(value, ignoreCase: false);

            Assert.False(result.HasValue);
        }

        [Theory]
        [InlineData("OneTwo")]
        [InlineData("onetwo")]
        [InlineData("onetWo")]
        [CLSCompliant(false)]
        public static void Enum_Succeeds_ForAliasValue_BitFlagsEnum(string value)
        {
            var result = ParseTo.Enum<My.EnumBits>("OneTwo");

            Assert.True(result.HasValue);
            Assert.Equal(My.EnumBits.OneTwo, result.Value);
        }

        [Theory]
        [InlineData("onetwo")]
        [InlineData("onetWo")]
        [CLSCompliant(false)]
        public static void Enum_ReturnsNull_ForAliasValue_BitFlagsEnum_DoNotIgnoreCase(string value)
        {
            var result = ParseTo.Enum<My.EnumBits>(value, ignoreCase: false);

            Assert.False(result.HasValue);
        }

        [Theory]
        [InlineData("One,Two")]
        [InlineData("one,two")]
        [InlineData("oNe,two")]
        [InlineData("one,tWo")]
        [InlineData("One,Two ")]
        [InlineData("one,two ")]
        [InlineData(" One,Two")]
        [InlineData(" one,two")]
        [InlineData(" One,Two ")]
        [InlineData(" one,two ")]
        [InlineData("One, Two")]
        [InlineData("One,  Two")]
        [InlineData("One,   Two")]
        [InlineData(" One, Two")]
        [InlineData(" One,  Two")]
        [InlineData(" One, Two ")]
        [InlineData(" One,  Two ")]
        [CLSCompliant(false)]
        public static void Enum_Succeeds_ForCompositeValue_BitFlagsEnum(string value)
        {
            var result = ParseTo.Enum<My.EnumBits>(value);

            Assert.True(result.HasValue);
            Assert.Equal(My.EnumBits.OneTwo, result.Value);
        }

        [Theory]
        [InlineData("one,two")]
        [InlineData("oNe,two")]
        [InlineData("one,tWo")]
        [InlineData("one,two ")]
        [InlineData(" one,two")]
        [InlineData(" one,two ")]
        [InlineData("one, two")]
        [InlineData("one,  two")]
        [InlineData("one, two ")]
        [InlineData(" one, two")]
        [InlineData(" one, two ")]
        [CLSCompliant(false)]
        public static void Enum_ReturnsNull_ForCompositeValue_BitFlagsEnum_DoNotignoreCase(string value)
        {
            var result = ParseTo.Enum<My.EnumBits>(value, ignoreCase: false);

            Assert.False(result.HasValue);
        }

        #endregion
    }
}
