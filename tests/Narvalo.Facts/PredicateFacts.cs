﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.TestCommon;
    using Xunit;

    public static partial class PredicateFacts
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

    public static partial class PredicateFacts
    {
        #region IsFlagsEnum()

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForNullInput()
        {
            // Arrange
            Type type = null;

            // Act & Assert
            Assert.False(Predicate.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsTrue_ForFlagsEnumInput()
        {
            // Arrange
            var type = typeof(MyFlagsEnum_);

            // Act & Assert
            Assert.True(Predicate.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForNonFlagsEnumInput()
        {
            // Arrange
            var type = typeof(MyEnum_);

            // Act & Assert
            Assert.False(Predicate.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForSimpleTypeInput()
        {
            // Arrange
            var type = typeof(Int32);

            // Act & Assert
            Assert.False(Predicate.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForNonEnumerationStructInput()
        {
            // Arrange
            var type = typeof(MyStruct_);

            // Act & Assert
            Assert.False(Predicate.IsFlagsEnum(type));
        }

        #endregion

        #region IsWhiteSpace()

        [Fact]
        public static void IsWhiteSpace_ReturnsTrue_ForEmptyString()
        {
            // Act & Assert
            Assert.True(Predicate.IsWhiteSpace(String.Empty));
        }

        [Fact]
        public static void IsWhiteSpace_ReturnsTrue_ForWhiteSpaceOnlyString()
        {
            // Act & Assert
            Assert.True(Predicate.IsWhiteSpace(Constants.WhiteSpaceOnlyString));
        }

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNullString()
        {
            // Act & Assert
            Assert.False(Predicate.IsWhiteSpace(null));
        }

        [Fact]
        public static void IsWhiteSpace_ReturnsFalse_ForNonEmptyOrWhiteSpaceString()
        {
            // Act & Assert
            Assert.False(Predicate.IsWhiteSpace("value"));
        }

        #endregion
    }
}
