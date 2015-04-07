// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static partial class TypeUtilityFacts
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

    public static partial class TypeUtilityFacts
    {
        #region IsEnum()

        [Fact]
        public static void IsEnum_IsTrue_ForEnum()
        {
            // Act & Assert
            Assert.True(TypeUtility.IsEnum<MyEnum_>());
        }

        [Fact]
        public static void IsEnum_IsTrue_ForFlagsEnum()
        {
            // Act & Assert
            Assert.True(TypeUtility.IsEnum<MyFlagsEnum_>());
        }

        [Fact]
        public static void IsEnum_IsFalse_ForSimpleType()
        {
            // Act & Assert
            Assert.False(TypeUtility.IsEnum<int>());
        }

        [Fact]
        public static void IsEnum_IsFalse_ForNonEnumerationStruct()
        {
            // Act & Assert
            Assert.False(TypeUtility.IsEnum<MyStruct_>());
        }

        #endregion

        #region IsFlagsEnum()

        [Fact]
        public static void IsFlagsEnum_IsFalse_ForNullParameter()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            Assert.False(TypeUtility.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_IsTrue_ForFlagsEnum()
        {
            // Act & Assert
            Assert.True(TypeUtility.IsFlagsEnum<MyFlagsEnum_>());
        }

        [Fact]
        public static void IsFlagsEnum_IsTrue_ForFlagsEnumParameter()
        {
            // Arrange
            var type = typeof(MyFlagsEnum_);

            // Act & Assert
            Assert.True(TypeUtility.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_IsFalse_ForNonFlagsEnum()
        {
            // Act & Assert
            Assert.False(TypeUtility.IsFlagsEnum<MyEnum_>());
        }

        [Fact]
        public static void IsFlagsEnum_IsFalse_ForNonFlagsEnumParameter()
        {
            // Arrange
            var type = typeof(MyEnum_);

            // Act & Assert
            Assert.False(TypeUtility.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_IsFalse_ForSimpleType()
        {
            // Act & Assert
            Assert.False(TypeUtility.IsFlagsEnum<int>());
        }

        [Fact]
        public static void IsFlagsEnum_IsFalse_ForSimpleTypeParameter()
        {
            // Arrange
            var type = typeof(int);

            // Act & Assert
            Assert.False(TypeUtility.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_IsFalse_ForNonEnumerationStruct()
        {
            // Act & Assert
            Assert.False(TypeUtility.IsFlagsEnum<MyStruct_>());
        }

        [Fact]
        public static void IsFlagsEnum_IsFalse_ForNonEnumerationStructParameter()
        {
            // Arrange
            var type = typeof(MyStruct_);

            // Act & Assert
            Assert.False(TypeUtility.IsFlagsEnum(type));
        }

        #endregion
    }
}
