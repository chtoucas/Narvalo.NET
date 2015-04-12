// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.TestCommon;
    using Xunit;

    public static partial class ParseToFacts
    {
        #region Boolean()

        [Fact]
        public static void Boolean_ReturnsNull_ForNullString()
        {
            // Act
            bool? result = ParseTo.Boolean(null, BooleanStyles.Default);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForEmptyString()
        {
            // Act
            bool? result = ParseTo.Boolean(String.Empty, BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsFalse_ForEmptyStringAndEmptyIsFalse()
        {
            // Act
            bool? result = ParseTo.Boolean(String.Empty, BooleanStyles.EmptyIsFalse);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(false, result.Value);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForLiteralTrueAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("true", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(true, result.Value);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForLiteralMixedCaseTrueAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("TrUe", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(true, result.Value);
        }

        [Fact]
        public static void Boolean_ReturnsFalse_ForLiteralFalseAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("false", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(false, result.Value);
        }

        [Fact]
        public static void Boolean_ReturnsFalse_ForLiteralMixedCaseFalseAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("fAlSe", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(false, result.Value);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForLiteralTrueAndWhiteSpacesAndLiteralStyle()
        {
            // Act
            bool? result = ParseTo.Boolean(" true ", BooleanStyles.Literal);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(true, result.Value);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForStrictlyPositiveInt32AndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("10", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsTrue_ForOneAndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("1", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(true, result.Value);
        }

        [Fact]
        public static void Boolean_ReturnsFalse_ForZeroAndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("0", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(false, result.Value);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForMinusOneAndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("-1", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForNegativeInt32AndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("-10", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Boolean_ReturnsNull_ForDecimalAndIntegerStyle()
        {
            // Act
            bool? result = ParseTo.Boolean("-10.1", BooleanStyles.ZeroOrOne);

            // Assert
            Assert.False(result.HasValue);
        }

        #endregion

        #region Enum()

        [Fact]
        public static void Enum_ThrowsArgumentException_ForInt32()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ParseTo.Enum<int>("Whatever"));
        }

        [Fact]
        public static void Enum_ThrowsArgumentException_ForNonEnumerationStruct()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ParseTo.Enum<My.EmptyStruct>("Whatever"));
        }

        [Fact]
        public static void Enum_ReturnsNull_ForInvalidValue()
        {
            // Act
            var result = ParseTo.Enum<My.SimpleEnum>("InvalidValue");

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForInvalidValueAndBadCase()
        {
            // Act
            var result = ParseTo.Enum<My.SimpleEnum>("InvalidValue", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValue()
        {
            // Act
            My.SimpleEnum? result = ParseTo.Enum<My.SimpleEnum>("ActualValue");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnum.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValueWithWhiteSpaces()
        {
            // Act
            My.SimpleEnum? result = ParseTo.Enum<My.SimpleEnum>(" ActualValue ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnum.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValueAndIgnoreCase()
        {
            // Act
            My.SimpleEnum? result = ParseTo.Enum<My.SimpleEnum>("actualvalue");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnum.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForActualValueAndBadCase()
        {
            // Act
            var result = ParseTo.Enum<My.SimpleEnum>("actualvalue", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValueWithWhiteSpacesAndIgnoreCase()
        {
            // Act
            My.SimpleEnum? result = ParseTo.Enum<My.SimpleEnum>(" actualvalue ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnum.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForActualValueWithWhiteSpacesAndBadCase()
        {
            // Act
            My.SimpleEnum? result = ParseTo.Enum<My.SimpleEnum>(" actualvalue ", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForAliasValue()
        {
            // Act
            var result = ParseTo.Enum<My.SimpleEnum>("AliasValue");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnum.AliasValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForAliasValueWithWhiteSpaces()
        {
            // Act
            My.SimpleEnum? result = ParseTo.Enum<My.SimpleEnum>(" AliasValue ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnum.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForAliasValueAndIgnoreCase()
        {
            // Act
            var result = ParseTo.Enum<My.SimpleEnum>("aliasvalue");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnum.AliasValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForAliasValueAndBadCase()
        {
            // Act
            var result = ParseTo.Enum<My.SimpleEnum>("aliasvalue", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForAliasValueWithWhiteSpacesAndIgnoreCase()
        {
            // Act
            My.SimpleEnum? result = ParseTo.Enum<My.SimpleEnum>(" aliasvalue ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnum.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForAliasValueWithWhiteSpacesAndBadCase()
        {
            // Act
            My.SimpleEnum? result = ParseTo.Enum<My.SimpleEnum>(" aliasvalue ", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForNamedCompositeValue()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>("CompositeValue1");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.FlagsEnum.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForNamedCompositeValueAndIgnoreCase()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>("compositeValue1");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.FlagsEnum.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForNamedCompositeValueAndBadCase()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>("compositeValue1", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForCompositeValue()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>("ActualValue1,ActualValue2");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.FlagsEnum.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForCompositeValueAndIgnoreCase()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>("actualValue1,actualValue2");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.FlagsEnum.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForCompositeValueAndBadCase()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>("actualValue1,actualValue2", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForCompositeValueWithWhiteSpaces()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>(" ActualValue1,  ActualValue2 ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.FlagsEnum.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForCompositeValueWithWhiteSpacesAndIgnoreCase()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>(" actualValue1,  actualValue2 ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.FlagsEnum.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForCompositeValueWithWhiteSpacesAndBadCase()
        {
            // Act
            My.FlagsEnum? result = ParseTo.Enum<My.FlagsEnum>(" actualValue1,  actualValue2 ", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        #endregion
    }
}
