// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Reflection.Emit;
    using NSubstitute;
    using Xunit;

    public static class CachedPresenterConstructorResolverFacts
    {
        public static class Ctor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullInnerResolver()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CachedPresenterConstructorResolver(inner: null));
            }
        }

        public static class ResolveMethod
        {
            [Fact]
            public static void CachesInnerResolverCalls()
            {
                // Arrange
                var inner = Substitute.For<IPresenterConstructorResolver>();
                inner.Resolve(typeof(String), typeof(Char[]))
                    .Returns(new DynamicMethod(String.Empty, typeof(String), new Type[0]));

                var resolver = new CachedPresenterConstructorResolver(inner);

                // Act
                resolver.Resolve(typeof(String), typeof(Char[]));
                resolver.Resolve(typeof(String), typeof(Char[]));

                // Assert
                inner.Received(1).Resolve(typeof(String), typeof(Char[]));
            }

            [Fact]
            public static void ReturnsSameResult_ForSameInput()
            {
                // Arrange
                var inner = Substitute.For<IPresenterConstructorResolver>();
                inner.Resolve(typeof(String), typeof(Char[]))
                    .Returns(new DynamicMethod(String.Empty, typeof(String), new Type[0]));

                var resolver = new CachedPresenterConstructorResolver(inner);

                // Act
                var ctor1 = resolver.Resolve(typeof(String), typeof(Char[]));
                var ctor2 = resolver.Resolve(typeof(String), typeof(Char[]));

                // Assert
                Assert.Equal(ctor1, ctor2);
            }

            [Fact]
            public static void ReturnsDifferentResults_ForDifferentInputs()
            {
                // Arrange
                var inner = Substitute.For<IPresenterConstructorResolver>();
                inner.Resolve(typeof(String), typeof(Char*))
                    .Returns(new DynamicMethod(String.Empty, typeof(String), new Type[0]));
                inner.Resolve(typeof(String), typeof(Char[]))
                    .Returns(new DynamicMethod(String.Empty, typeof(String), new Type[0]));

                var resolver = new CachedPresenterConstructorResolver(inner);

                // Act
                var ctor1 = resolver.Resolve(typeof(String), typeof(Char*));
                var ctor2 = resolver.Resolve(typeof(String), typeof(Char[]));

                // Assert
                Assert.NotEqual(ctor1, ctor2);
            }
        }
    }
}
