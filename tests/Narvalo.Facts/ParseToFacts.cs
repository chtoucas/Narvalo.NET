// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class ParseToFacts
    {
        private enum MyEnum_
        {
            None = 0,
            ActualValue = 1,
            AliasValue = ActualValue,
        }

        [Flags]
        private enum MyFlagsEnum_
        {
            None = 0,
            ActualValue1 = 1 << 0,
            ActualValue2 = 1 << 1,
            ActualValue3 = 1 << 2,
            CompositeValue1 = ActualValue1 | ActualValue2,
            CompositeValue2 = ActualValue1 | ActualValue2 | ActualValue3
        }

        private struct MyStruct_ { }
    }

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
            Assert.Throws<ArgumentException>(() => ParseTo.Enum<MyStruct_>("Whatever"));
        }

        [Fact]
        public static void Enum_ReturnsNull_ForInvalidValue()
        {
            // Act
            var result = ParseTo.Enum<MyEnum_>("InvalidValue");

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForInvalidValueAndBadCase()
        {
            // Act
            var result = ParseTo.Enum<MyEnum_>("InvalidValue", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValue()
        {
            // Act
            MyEnum_? result = ParseTo.Enum<MyEnum_>("ActualValue");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValueWithWhiteSpaces()
        {
            // Act
            MyEnum_? result = ParseTo.Enum<MyEnum_>(" ActualValue ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValueAndIgnoreCase()
        {
            // Act
            MyEnum_? result = ParseTo.Enum<MyEnum_>("actualvalue");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForActualValueAndBadCase()
        {
            // Act
            var result = ParseTo.Enum<MyEnum_>("actualvalue", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValueWithWhiteSpacesAndIgnoreCase()
        {
            // Act
            MyEnum_? result = ParseTo.Enum<MyEnum_>(" actualvalue ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForActualValueWithWhiteSpacesAndBadCase()
        {
            // Act
            MyEnum_? result = ParseTo.Enum<MyEnum_>(" actualvalue ", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForAliasValue()
        {
            // Act
            var result = ParseTo.Enum<MyEnum_>("AliasValue");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.AliasValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForAliasValueWithWhiteSpaces()
        {
            // Act
            MyEnum_? result = ParseTo.Enum<MyEnum_>(" AliasValue ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForAliasValueAndIgnoreCase()
        {
            // Act
            var result = ParseTo.Enum<MyEnum_>("aliasvalue");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.AliasValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForAliasValueAndBadCase()
        {
            // Act
            var result = ParseTo.Enum<MyEnum_>("aliasvalue", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForAliasValueWithWhiteSpacesAndIgnoreCase()
        {
            // Act
            MyEnum_? result = ParseTo.Enum<MyEnum_>(" aliasvalue ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.ActualValue, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForAliasValueWithWhiteSpacesAndBadCase()
        {
            // Act
            MyEnum_? result = ParseTo.Enum<MyEnum_>(" aliasvalue ", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForNamedCompositeValue()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>("CompositeValue1");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyFlagsEnum_.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForNamedCompositeValueAndIgnoreCase()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>("compositeValue1");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyFlagsEnum_.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForNamedCompositeValueAndBadCase()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>("compositeValue1", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForCompositeValue()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>("ActualValue1,ActualValue2");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyFlagsEnum_.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForCompositeValueAndIgnoreCase()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>("actualValue1,actualValue2");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyFlagsEnum_.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForCompositeValueAndBadCase()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>("actualValue1,actualValue2", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForCompositeValueWithWhiteSpaces()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>(" ActualValue1,  ActualValue2 ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyFlagsEnum_.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForCompositeValueWithWhiteSpacesAndIgnoreCase()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>(" actualValue1,  actualValue2 ");

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyFlagsEnum_.CompositeValue1, result.Value);
        }

        [Fact]
        public static void Enum_ReturnsNull_ForCompositeValueWithWhiteSpacesAndBadCase()
        {
            // Act
            MyFlagsEnum_? result = ParseTo.Enum<MyFlagsEnum_>(" actualValue1,  actualValue2 ", ignoreCase: false);

            // Assert
            Assert.False(result.HasValue);
        }

        #endregion
    }
}
