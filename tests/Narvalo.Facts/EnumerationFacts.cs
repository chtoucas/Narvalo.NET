// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public static partial class EnumerationFacts
    {
        private enum EnumStub_
        {
            None = 0,
            ActualValue = 1,
            AliasValue = ActualValue,
        }

        private enum EnumWithoutDefaultStub_
        {
            ActualValue1 = 1,
            ActualValue2 = 2,
        }

        [Flags]
        private enum EnumFlagStub_
        {
            None = 0,
            ActualValue1 = 1 << 0,
            ActualValue2 = 1 << 1,
            ActualValue3 = 1 << 2,
            CompositeValue1 = ActualValue1 | ActualValue2,
            CompositeValue2 = ActualValue1 | ActualValue2 | ActualValue3
        }

        private struct StructStub_ { }
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
            StructStub_ result;
            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => Enumeration.TryParse<StructStub_>("Whatever", out result));
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValue()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>("ActualValue", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumStub_.ActualValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValueAndWhitespaces()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>(" ActualValue  ", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumStub_.ActualValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValueAndIgnoreCase()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>(
                "actualvalue", true /* ignoreCase */, out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumStub_.ActualValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValueAndIgnoreCaseAndWhitespaces()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>(
                "  actualvalue   ", true /* ignoreCase */, out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumStub_.ActualValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForActualValueAndBadCase()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>(
                "actualvalue", false /* ignoreCase */, out result);
            // Assert
            Assert.False(succeed);
            Assert.NotEqual(EnumStub_.ActualValue, result);
            Assert.Equal(default(EnumStub_), result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForAliasValue()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>("AliasValue", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumStub_.AliasValue, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualValueAndFlagEnum()
        {
            // Arrange
            EnumFlagStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumFlagStub_>("ActualValue1", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumFlagStub_.ActualValue1, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForActualCompositeValueAndFlagEnum()
        {
            // Arrange
            EnumFlagStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumFlagStub_>("CompositeValue1", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumFlagStub_.CompositeValue1, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForUserDefinedCompositeValueAndFlagEnum()
        {
            // Arrange
            EnumFlagStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumFlagStub_>("ActualValue1, ActualValue2", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumFlagStub_.CompositeValue1, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForUserDefinedCompositeValueAndWhitespacesAndFlagEnum()
        {
            // Arrange
            EnumFlagStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumFlagStub_>(
                "  ActualValue1,  ActualValue2,ActualValue3  ", out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumFlagStub_.CompositeValue2, result);
        }

        [Fact]
        public static void TryParse_ReturnsTrueAndPicksCorrectMember_ForUserDefinedCompositeValueAndWhitespacesAndIgnoreCaseAndFlagEnum()
        {
            // Arrange
            EnumFlagStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumFlagStub_>(
                "  actualvalue1,  ActualValue2,actualVAlue3  ", true /* ignoreCase */, out result);
            // Assert
            Assert.True(succeed);
            Assert.Equal(EnumFlagStub_.CompositeValue2, result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidValue()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>("InvalidValue", out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(EnumStub_), result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidValueAndIgnoreCase()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>("invalidvalue", true /* ignoreCase */, out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(EnumStub_), result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidValueAndBadCase()
        {
            // Arrange
            EnumStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumStub_>("invalidvalue", false /* ignoreCase */, out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(EnumStub_), result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidValueAndEnumWithoutDefault()
        {
            // Arrange
            EnumWithoutDefaultStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumWithoutDefaultStub_>("InvalidValue", out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(EnumWithoutDefaultStub_), result);
        }

        [Fact]
        public static void TryParse_ReturnsFalseAndPicksDefaultMember_ForInvalidUserDefinedCompositeValueAndWhitespacesAndFlagEnum()
        {
            // Arrange
            EnumFlagStub_ result;
            // Act
            var succeed = Enumeration.TryParse<EnumFlagStub_>("  InvalidValue,  ActualValue2,ActualValue3  ", out result);
            // Assert
            Assert.False(succeed);
            Assert.Equal(default(EnumFlagStub_), result);
        }

#endif

        #endregion
    }
}
