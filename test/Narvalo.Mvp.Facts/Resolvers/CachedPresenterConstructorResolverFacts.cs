// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Reflection.Emit;
    using NSubstitute;
    using Xunit;

    public static class CachedPresenterConstructorResolverFacts
    {
        public static class TheConstructorMethod
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullInnerResolver()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CachedPresenterConstructorResolver(inner: null));
            }
        }

        public static class TheResolverMethod
        {
            [Fact]
            public static void AlwaysReturnSameConstructorMethod()
            {
                // Arrange
                var input = Tuple.Create(typeof(String), typeof(Char[]));

                var inner = Substitute.For<IPresenterConstructorResolver>();
                inner.Resolve(input).Returns(new DynamicMethod(String.Empty, typeof(String), new Type[0]));

                var resolver = new CachedPresenterConstructorResolver(inner);

                // Act
                var ctor1 = resolver.Resolve(input);
                var ctor2 = resolver.Resolve(input);

                // Assert
                Assert.Equal(ctor1, ctor2);
            }

            [Fact]
            public static void ReturnsDifferentConstructorMethods_ForDifferentTypes()
            {
                // Arrange
                var input1 = Tuple.Create(typeof(String), typeof(Char*));
                var input2 = Tuple.Create(typeof(String), typeof(Char[]));

                var inner = Substitute.For<IPresenterConstructorResolver>();
                inner.Resolve(input1).Returns(new DynamicMethod(String.Empty, typeof(String), new Type[0]));
                inner.Resolve(input2).Returns(new DynamicMethod(String.Empty, typeof(String), new Type[0]));

                var resolver = new CachedPresenterConstructorResolver(inner);

                // Act
                var ctor1 = resolver.Resolve(input1);
                var ctor2 = resolver.Resolve(input2);

                // Assert
                Assert.NotEqual(ctor1, ctor2);
            }
        }
    }
}
