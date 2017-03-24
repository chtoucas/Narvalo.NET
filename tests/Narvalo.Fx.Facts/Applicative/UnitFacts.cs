// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    using Xunit;

    public static class UnitFacts
    {
        [Fact]
        public static void Equality_Tests()
        {
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.True(u1 == Unit.Default);
            Assert.True(Unit.Default == u1);
            Assert.True(u1 == u2);
        }

        [Fact]
        public static void Inequality_Tests()
        {
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.False(u1 != Unit.Default);
            Assert.False(Unit.Default != u1);
            Assert.False(u1 != u2);
        }

        [Fact]
        public static void Equals_Tests()
        {
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.True(u1.Equals(u1));
            Assert.True(u1.Equals(u2));
            Assert.True(u1.Equals(Unit.Default));
            Assert.True(Unit.Default.Equals(u1));
            Assert.True(Unit.Default.Equals(u2));
            Assert.True(Unit.Default.Equals(Unit.Default));

            Assert.False(u1.Equals(null));
            Assert.False(u1.Equals(new Object()));
            Assert.False(new Object().Equals(u1));
            Assert.False(new Object().Equals(Unit.Default));
        }

        [Fact]
        public static void GetHashCode_Tests()
        {
            Assert.Equal(0, new Unit().GetHashCode());
            Assert.Equal(0, Unit.Default.GetHashCode());
        }

        [Fact]
        public static void ToString_Tests()
        {
            Assert.Equal("()", new Unit().ToString());
            Assert.Equal("()", Unit.Default.ToString());
        }
    }
}
