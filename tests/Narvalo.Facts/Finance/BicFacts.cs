// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Xunit;

    public static partial class BicFacts
    {
        [Fact]
        public static void InstitutionCode()
        {
            // Arrange
            var bic1 = Bic.Parse("ABCDXXXX");
            var bic2 = Bic.Parse("ABCDXXXXXXX");

            // Act & Assert
            Assert.Equal("ABCD", bic1.InstitutionCode);
            Assert.Equal("ABCD", bic2.InstitutionCode);
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("123456789_1")]
        [CLSCompliant(false)]
        public static void TryParse_Succeeds(string value)
            => Assert.True(Bic.TryParse(value).HasValue);

        [Theory]
        [InlineData("12345678")]
        [InlineData("123456789_1")]
        [CLSCompliant(false)]
        public static void Parse_Succeeds(string value)
            => Bic.Parse(value);

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        [InlineData("123456")]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("123456789_")]
        [InlineData("123456789_12")]
        [CLSCompliant(false)]
        public static void TryParse_ReturnsNull(string value)
            => Assert.False(Bic.TryParse(value).HasValue);

        [Fact]
        public static void Parse_ThrowsArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => Bic.Parse(null));

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        [InlineData("123456")]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("123456789_")]
        [InlineData("123456789_12")]
        [CLSCompliant(false)]
        public static void Parse_ThrowsFormatException(string value)
            => Assert.Throws<FormatException>(() => Bic.Parse(value));
    }
}
