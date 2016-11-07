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

        [Fact]
        public static void TryParse()
        {
            Assert.False(Bic.TryParse(null).HasValue);
            Assert.False(Bic.TryParse("XXXXXXX").HasValue);
            Assert.False(Bic.TryParse("XXXXXXXXX").HasValue);
            Assert.False(Bic.TryParse("XXXXXXXXXX").HasValue);
            Assert.False(Bic.TryParse("XXXXXXXXXXXX").HasValue);
        }

        [Fact]
        public static void Parse()
        {
            Assert.Throws<ArgumentNullException>(() => Bic.Parse(null));
            Assert.Throws<FormatException>(() => Bic.Parse("XXXXXXX"));
            Assert.Throws<FormatException>(() => Bic.Parse("XXXXXXXXX"));
            Assert.Throws<FormatException>(() => Bic.Parse("XXXXXXXXXX"));
            Assert.Throws<FormatException>(() => Bic.Parse("XXXXXXXXXXXX"));
        }
    }
}
