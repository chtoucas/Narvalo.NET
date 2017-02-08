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
            => Assert.Throws<ArgumentException>(() => ConvertTo.Enum<int>(1));

        [Fact]
        public static void Enum_ThrowsArgumentException_ForNonEnumerationStruct()
            => Assert.Throws<ArgumentException>(() => ConvertTo.Enum<My.EmptyStruct>(1));

        [Fact]
        public static void Enum_ThrowsNotSupportedException_ForBitFlagsEnum()
            => Assert.Throws<NotSupportedException>(() => ConvertTo.Enum<My.EnumBits>(1));

        [Fact]
        public static void Enum_ReturnsNull_ForInvalidInput()
        {
            var result = ConvertTo.Enum<My.Enum012>(3);

            Assert.False(result.HasValue);
        }

        [Theory]
        [InlineData(0, My.Enum012.Zero)]
        [InlineData(1, My.Enum012.One)]
        [InlineData(2, My.Enum012.Two)]
        [CLSCompliant(false)]
        public static void Enum_Succeeds_ForValidInput(int value, My.Enum012 expectedValue)
        {
            var result = ConvertTo.Enum<My.Enum012>(value);

            Assert.True(result.HasValue);
            Assert.Equal(expectedValue, result.Value);
        }

        #endregion
    }
}
