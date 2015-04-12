// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.TestCommon;
    using Xunit;

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
            var type = typeof(My.FlagsEnum);

            // Act & Assert
            Assert.True(Predicate.IsFlagsEnum(type));
        }

        [Fact]
        public static void IsFlagsEnum_ReturnsFalse_ForNonFlagsEnumInput()
        {
            // Arrange
            var type = typeof(My.SimpleEnum);

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
            var type = typeof(My.EmptyStruct);

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
