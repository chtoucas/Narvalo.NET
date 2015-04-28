// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    using NSubstitute;
    using Xunit;

    public static class CachedCompositeViewTypeResolverFacts
    {
        #region Ctor()

        [Fact]
        public static void Ctor_ThrowsArgumentNullException_ForNullInnerResolver()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CachedCompositeViewTypeResolver(inner: null));
        }

        #endregion

        #region Resolve()

        [Fact]
        public static void Resolve_CachesInnerResolverCalls()
        {
            // Arrange
            var inner = Substitute.For<ICompositeViewTypeResolver>();
            inner.Resolve(typeof(String)).Returns(typeof(Int32));

            var resolver = new CachedCompositeViewTypeResolver(inner);

            // Act
            resolver.Resolve(typeof(String));
            resolver.Resolve(typeof(String));

            // Assert
            inner.Received(1).Resolve(typeof(String));
            Assert.True(true);
        }

        [Fact]
        public static void Resolve_ReturnsSameResult_ForSameInput()
        {
            // Arrange
            var inner = Substitute.For<ICompositeViewTypeResolver>();
            inner.Resolve(typeof(String)).Returns(typeof(Int32));

            var resolver = new CachedCompositeViewTypeResolver(inner);

            // Act
            var result1 = resolver.Resolve(typeof(String));
            var result2 = resolver.Resolve(typeof(String));

            // Assert
            Assert.Equal(result1, result2);
        }

        [Fact]
        public static void Resolve_ReturnsDifferentResults_ForDifferentInputs()
        {
            // Arrange
            var inner = Substitute.For<ICompositeViewTypeResolver>();
            inner.Resolve(typeof(String)).Returns(typeof(Int32));
            inner.Resolve(typeof(Int32)).Returns(typeof(String));

            var resolver = new CachedCompositeViewTypeResolver(inner);

            // Act
            var result1 = resolver.Resolve(typeof(String));
            var result2 = resolver.Resolve(typeof(Int32));

            // Assert
            Assert.NotEqual(result1, result2);
        }

        #endregion
    }
}
