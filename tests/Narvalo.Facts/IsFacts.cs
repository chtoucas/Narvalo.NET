// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class IsFacts
    {
        private const string WHITESPACE_ONLY_STRING = "     ";

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

        private sealed class MyValue_
        {
            private readonly int _value;

            public MyValue_(int value)
            {
                _value = value;
            }

            public int Value { get { return _value; } }
        }
    }

    public static partial class IsFacts
    {
        #region Enum()

        [Fact]
        public static void Enum_ReturnsTrue_ForEnum()
        {
            // Act & Assert
            Assert.True(Is.Enum<MyEnum_>());
        }

        [Fact]
        public static void Enum_ReturnsTrue_ForFlagsEnum()
        {
            // Act & Assert
            Assert.True(Is.Enum<MyFlagsEnum_>());
        }

        [Fact]
        public static void Enum_ReturnsFalse_ForSimpleType()
        {
            // Act & Assert
            Assert.False(Is.Enum<int>());
        }

        [Fact]
        public static void Enum_ReturnsFalse_ForNonEnumerationStruct()
        {
            // Act & Assert
            Assert.False(Is.Enum<MyStruct_>());
        }

        #endregion

        #region FlagsEnum()

        [Fact]
        public static void FlagsEnum_ReturnsFalse_ForNullParameter()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            Assert.False(Is.FlagsEnum(type));
        }

        [Fact]
        public static void FlagsEnum_ReturnsTrue_ForFlagsEnum()
        {
            // Act & Assert
            Assert.True(Is.FlagsEnum<MyFlagsEnum_>());
        }

        [Fact]
        public static void FlagsEnum_ReturnsTrue_ForFlagsEnumParameter()
        {
            // Arrange
            var type = typeof(MyFlagsEnum_);

            // Act & Assert
            Assert.True(Is.FlagsEnum(type));
        }

        [Fact]
        public static void FlagsEnum_ReturnsFalse_ForNonFlagsEnum()
        {
            // Act & Assert
            Assert.False(Is.FlagsEnum<MyEnum_>());
        }

        [Fact]
        public static void FlagsEnum_ReturnsFalse_ForNonFlagsEnumParameter()
        {
            // Arrange
            var type = typeof(MyEnum_);

            // Act & Assert
            Assert.False(Is.FlagsEnum(type));
        }

        [Fact]
        public static void FlagsEnum_ReturnsFalse_ForSimpleType()
        {
            // Act & Assert
            Assert.False(Is.FlagsEnum<int>());
        }

        [Fact]
        public static void FlagsEnum_ReturnsFalse_ForSimpleTypeParameter()
        {
            // Arrange
            var type = typeof(int);

            // Act & Assert
            Assert.False(Is.FlagsEnum(type));
        }

        [Fact]
        public static void FlagsEnum_ReturnsFalse_ForNonEnumerationStruct()
        {
            // Act & Assert
            Assert.False(Is.FlagsEnum<MyStruct_>());
        }

        [Fact]
        public static void FlagsEnum_ReturnsFalse_ForNonEnumerationStructParameter()
        {
            // Arrange
            var type = typeof(MyStruct_);

            // Act & Assert
            Assert.False(Is.FlagsEnum(type));
        }

        #endregion

        #region WhiteSpace()

        [Fact]
        public static void WhiteSpace_ThrowsArgumentNullException_ForNullString()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Is.WhiteSpace(null));
        }

        [Fact]
        public static void WhiteSpace_ReturnsTrue_ForEmptyString()
        {
            // Act & Assert
            Assert.True(Is.WhiteSpace(String.Empty));
        }

        [Fact]
        public static void WhiteSpace_ReturnsTrue_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.True(Is.WhiteSpace(WHITESPACE_ONLY_STRING));
        }

        [Fact]
        public static void WhiteSpace_ReturnsFalse_ForNonEmptyOrWhiteSpaceString()
        {
            // Act & Assert
            Assert.False(Is.WhiteSpace("value"));
        }

        #endregion
    }
}
