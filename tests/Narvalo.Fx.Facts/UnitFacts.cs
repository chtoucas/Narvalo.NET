// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    using Xunit;

    public static class UnitFacts
    {
        [Fact]
        public static void Equality_Tests()
        {
            // Arrange
            var u1 = new Unit();
            var u2 = new Unit();

            // Act & Assert
            Assert.True(u1 == Unit.Single);
            Assert.True(Unit.Single == u1);
            Assert.True(u1 == u2);
        }

        [Fact]
        public static void Inequality_Tests()
        {
            // Arrange
            var u1 = new Unit();
            var u2 = new Unit();

            // Act & Assert
            Assert.False(u1 != Unit.Single);
            Assert.False(Unit.Single != u1);
            Assert.False(u1 != u2);
        }

        [Fact]
        public static void Equals_Tests()
        {
            // Arrange
            var u1 = new Unit();
            var u2 = new Unit();

            // Act & Assert
            Assert.True(u1.Equals(u1));
            Assert.True(u1.Equals(u2));
            Assert.True(u1.Equals(Unit.Single));
            Assert.True(Unit.Single.Equals(u1));
            Assert.True(Unit.Single.Equals(u2));
            Assert.True(Unit.Single.Equals(Unit.Single));

            Assert.False(u1.Equals(null));
            Assert.False(u1.Equals(new Object()));
            Assert.False(new Object().Equals(u1));
            Assert.False(new Object().Equals(Unit.Single));
        }

        [Fact]
        public static void GetHashCode_Tests()
        {
            // Act & Assert
            Assert.Equal(0, new Unit().GetHashCode());
            Assert.Equal(0, Unit.Single.GetHashCode());
        }

        [Fact]
        public static void ToString_Tests()
        {
            // Act & Assert
            Assert.Equal("()", new Unit().ToString());
            Assert.Equal("()", Unit.Single.ToString());
        }
    }
}
