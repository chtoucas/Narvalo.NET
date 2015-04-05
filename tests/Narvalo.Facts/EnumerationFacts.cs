// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public static partial class EnumerationFacts
    {
        private enum MyEnum_
        {
            None = 0,
            ActualValue = 1,
            AliasValue = ActualValue,
        }

        private enum MyEnumWithoutDefault_
        {
            ActualValue1 = 1,
            ActualValue2 = 2,
        }

        [Flags]
        private enum MyBitwiseEnum_
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

    public static partial class EnumerationFacts
    {
        #region TryParse()

#if NET_35

        [Fact]
        public static void TryParse_ThrowsArgumentException_ForInt32()
        {
            // Arrange
            int result;
            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => Enumeration.TryParse<int>("Whatever", out result));
        }

        [Fact]
        public static void TryParse_ThrowsArgumentException_ForNonEnumerationStruct()
        {
            // Arrange
            MyStruct_ result;
            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => Enumeration.TryParse<MyStruct_>("Whatever", out result));
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValue()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>("ActualValue", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyEnum_.ActualValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValueAndWhitespaces()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>(" ActualValue  ", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyEnum_.ActualValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValueAndIgnoreCase()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>(
                "actualvalue", true /* ignoreCase */, out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyEnum_.ActualValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValueAndIgnoreCaseAndWhitespaces()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>(
                "  actualvalue   ", true /* ignoreCase */, out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyEnum_.ActualValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForActualValueAndBadCase()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>(
                "actualvalue", false /* ignoreCase */, out result);
            // Assert
            Assert.False(succeed);
            Assert.NotEqual(MyEnum_.ActualValue, result);
            Assert.Equal(default(MyEnum_), result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForAliasValue()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>("AliasValue", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyEnum_.AliasValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValueAndBitwiseEnum()
        {
            // Arrange
            MyBitwiseEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyBitwiseEnum_>("ActualValue1", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyBitwiseEnum_.ActualValue1, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualCompositeValueAndBitwiseEnum()
        {
            // Arrange
            MyBitwiseEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyBitwiseEnum_>("CompositeValue1", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyBitwiseEnum_.CompositeValue1, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForUserDefinedCompositeValueAndBitwiseEnum()
        {
            // Arrange
            MyBitwiseEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyBitwiseEnum_>("ActualValue1, ActualValue2", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyBitwiseEnum_.CompositeValue1, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForUserDefinedCompositeValueAndWhitespacesAndBitwiseEnum()
        {
            // Arrange
            MyBitwiseEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyBitwiseEnum_>(
                "  ActualValue1,  ActualValue2,ActualValue3  ", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyBitwiseEnum_.CompositeValue2, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForUserDefinedCompositeValueAndWhitespacesAndIgnoreCaseAndBitwiseEnum()
        {
            // Arrange
            MyBitwiseEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyBitwiseEnum_>(
                "  actualvalue1,  ActualValue2,actualVAlue3  ", true /* ignoreCase */, out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(MyBitwiseEnum_.CompositeValue2, result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidValue()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>("InvalidValue", out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(MyEnum_), result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidValueAndIgnoreCase()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>("invalidvalue", true /* ignoreCase */, out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(MyEnum_), result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidValueAndBadCase()
        {
            // Arrange
            MyEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnum_>("invalidvalue", false /* ignoreCase */, out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(MyEnum_), result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidValueAndEnumWithoutDefault()
        {
            // Arrange
            MyEnumWithoutDefault_ result;
            // Act
            var succeed = Enumeration.TryParse<MyEnumWithoutDefault_>("InvalidValue", out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(MyEnumWithoutDefault_), result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidUserDefinedCompositeValueAndWhitespacesAndBitwiseEnum()
        {
            // Arrange
            MyBitwiseEnum_ result;
            // Act
            var succeed = Enumeration.TryParse<MyBitwiseEnum_>("  InvalidValue,  ActualValue2,ActualValue3  ", out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(MyBitwiseEnum_), result);
        }

#endif

        #endregion
    }
}
