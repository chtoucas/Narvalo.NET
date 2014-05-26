// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using NSubstitute;
    using Xunit;

    public static class CachedCompositeViewTypeResolverFacts
    {
        public static class Ctor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullInnerResolver()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CachedCompositeViewTypeResolver(inner: null));
            }
        }

        public static class ResolveMethod
        {
            [Fact]
            public static void CachesInnerResolverCalls()
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
            }

            [Fact]
            public static void ReturnsSameResult_ForSameInput()
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
            public static void ReturnsDifferentResults_ForDifferentInputs()
            {
                // Arrange
                var inner = Substitute.For<ICompositeViewTypeResolver>();
                inner.Resolve(typeof(String)).Returns(typeof(Int32));
                inner.Resolve(typeof(Int32)).Returns(typeof(Int32));

                var resolver = new CachedCompositeViewTypeResolver(inner);

                // Act
                var result1 = resolver.Resolve(typeof(String));
                var result2 = resolver.Resolve(typeof(Int32));

                // Assert
                Assert.NotEqual(result1, result2);
            }
        }
    }
}
