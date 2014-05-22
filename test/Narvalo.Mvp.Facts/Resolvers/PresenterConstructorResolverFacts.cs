// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using Xunit;

    public static class PresenterConstructorResolverFacts
    {
        #region Stubs

        public class PresenterStub<T>
        {
            public PresenterStub(T view)
            {
                View = view;
            }

            public T View { get; private set; }
        }

        public class BadPresenterStub<T>
        {
            public BadPresenterStub(T view, int param) { }
        }

        #endregion

        public static class TheResolverMethod
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullInput()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => resolver.Resolve(input: null));
            }

            [Fact]
            public static void ThrowsArgumentException_ForViewTypeMismatch()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var presenterType = typeof(PresenterStub<String>);
                var viewType = typeof(Int32);
                var input = Tuple.Create(presenterType, viewType);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => resolver.Resolve(input));
            }

            [Fact]
            public static void ThrowsArgumentException_WhenMissingConstructor()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var presenterType = typeof(BadPresenterStub<String>);
                var viewType = typeof(String);
                var input = Tuple.Create(presenterType, viewType);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => resolver.Resolve(input));
            }

            [Fact]
            public static void ReturnsConstructorMethodWithOneParameter()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var presenterType = typeof(PresenterStub<String>);
                var viewType = typeof(String);
                var input = Tuple.Create(presenterType, viewType);

                // Act
                var ctor = resolver.Resolve(input);
                var instance = ctor.Invoke(null, new[] { "test" });

                // Assert
                Assert.True(instance is PresenterStub<String>);
                Assert.Equal("test", ((PresenterStub<String>)instance).View);
            }

            [Fact]
            public static void ReturnsDifferentConstructorMethods()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var input1 = Tuple.Create(typeof(String), typeof(Char*));
                var input2 = Tuple.Create(typeof(String), typeof(Char[]));

                // Act
                var ctor1 = resolver.Resolve(input1);
                var ctor2 = resolver.Resolve(input2);

                // Assert
                Assert.NotEqual(ctor1, ctor2);
            }
        }
    }
}
