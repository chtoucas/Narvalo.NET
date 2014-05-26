// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using Xunit;

    public static class CompositeViewTypeResolverFacts
    {
        public static class ResolveMethod
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullViewType()
            {
                // Arrange
                var resolver = new CompositeViewTypeResolver();

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => resolver.Resolve(viewType: null));
            }
        }

        public static class ValidateViewTypeMethod
        {
            [Fact]
            public static void ThrowsArgumentException_ForNonInterfaceType()
            {
                // Arrange
                var viewType = typeof(Object);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
            }

            [Fact]
            public static void ThrowsArgumentException_ForInterfaceNotInheritingIView()
            {
                // Arrange
                var viewType = typeof(IDisposable);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
            }

            [Fact]
            public static void ThrowsArgumentException_ForPrivateInterface()
            {
                // Arrange
                var viewType = typeof(IPrivateView);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
            }

            [Fact]
            public static void ThrowsArgumentException_ForInterfaceWithPublicMethod()
            {
                // Arrange
                var viewType = typeof(IMyViewWithMethod);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
            }

            [Fact]
            public static void Passes_ForViewTypeOfIViewType()
            {
                // Arrange
                var viewType = typeof(IView);

                // Act
                CompositeViewTypeResolver.ValidateViewType(viewType);

                // Assert
                Assert.True(true);
            }

            [Fact]
            public static void Passes_ForViewTypeOfIViewTypeDefiningPropertyAndEventHandler()
            {
                // Arrange
                var viewType = typeof(IMyView);

                // Act
                CompositeViewTypeResolver.ValidateViewType(viewType);

                // Assert
                Assert.True(true);
            }

            [Fact]
            public static void Passes_ForViewTypeOfGenericIViewType()
            {
                // Arrange
                var viewType = typeof(IView<Object>);

                // Act
                CompositeViewTypeResolver.ValidateViewType(viewType);

                // Assert
                Assert.True(true);
            }
        }

        public interface IMyView : IView
        {
            event EventHandler MyHandler;

            string MyProperty { get; set; }
        }

        public interface IMyViewWithMethod : IView
        {
            void MyMethod();
        }

        interface IPrivateView : IView { }
    }
}
