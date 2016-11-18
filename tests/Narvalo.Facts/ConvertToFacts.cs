// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

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
            Assert.Throws<ArgumentException>(() => ConvertTo.Enum<My.EmptyStruct>(1));
        }

        [Fact]
        public static void Enum_ThrowsNotSupportedException_FoFlagEnum()
        {
            // Act & Assert
            Assert.Throws<NotSupportedException>(() => ConvertTo.Enum<My.BitwiseEnumeration>(1));
        }

        [Fact]
        public static void Enum_ReturnsNull_ForInvalidValue()
        {
            // Act
            var result = ConvertTo.Enum<My.SimpleEnumeration>(2);

            // Assert
            Assert.False(result.HasValue);
        }

        [Fact]
        public static void Enum_ReturnsExpectedValue_ForActualValue()
        {
            // Act
            var result = ConvertTo.Enum<My.SimpleEnumeration>(1);

            // Assert
            Assert.True(result.HasValue);
            Assert.Equal(My.SimpleEnumeration.ActualValue, result.Value);
        }

        #endregion
    }
}
