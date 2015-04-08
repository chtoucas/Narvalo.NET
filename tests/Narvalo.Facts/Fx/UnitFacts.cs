// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    using Xunit;

    public static class UnitFacts
    {
        #region op_Equality()

        [Fact]
        public static void Equality_AlwaysReturnsTrue()
        {
            // Act & Assert
            Assert.True(new Unit() == Unit.Single);
            Assert.True(Unit.Single == new Unit());
            Assert.True(new Unit() == new Unit());
        }

        #endregion

        #region op_Inequality()

        [Fact]
        public static void Inequality_AlwaysReturnsFalse()
        {
            // Act & Assert
            Assert.False(new Unit() != Unit.Single);
            Assert.False(Unit.Single != new Unit());
            Assert.False(new Unit() != new Unit());
        }

        #endregion

        #region Equals()

        [Fact]
        public static void Equals()
        {
            // Act & Assert
            Assert.True(new Unit().Equals(new Unit()));
            Assert.True(new Unit().Equals(Unit.Single));
            Assert.True(Unit.Single.Equals(new Unit()));
            Assert.True(Unit.Single.Equals(Unit.Single));
            Assert.False(Unit.Single.Equals(new Object()));
            Assert.False(new Object().Equals(Unit.Single));
        }

        #endregion
    }
}
