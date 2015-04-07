// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class ConvertToFacts
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

    public static partial class ConvertToFacts
    {
        #region Enum()

        [Fact]
        public static void Enum_ThrowsArgumentException_ForInt32()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ConvertTo.Enum<int>(1));
        }

        [Fact]
        public static void Enum_ThrowsArgumentException_ForNonEnumerationStruct()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ConvertTo.Enum<MyStruct_>(1));
        }

        [Fact]
        public static void Enum_ThrowsArgumentException_ForFlagsEnum()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ConvertTo.Enum<MyFlagsEnum_>(1));
        }

        [Fact]
        public static void Enum_ReturnsNull_ForInvalidValue()
        {
            // Act
            var result = ConvertTo.Enum<MyEnum_>(2);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValue()
        {
            // Act
            var result = ConvertTo.Enum<MyEnum_>(1);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(MyEnum_.ActualValue, result.Value);
        }

        #endregion
    }
}
