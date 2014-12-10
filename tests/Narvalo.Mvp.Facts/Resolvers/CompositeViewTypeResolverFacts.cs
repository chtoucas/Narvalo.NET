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
#if NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void ThrowsArgumentException_ForViewTypeOfClassType()
            {
                // Arrange
                var viewType = typeof(Object);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
            }
#endif

#if NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void ThrowsArgumentException_ForViewTypeNotInheritingIView()
            {
                // Arrange
                var viewType = typeof(IDisposable);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
            }
#endif

#if NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void ThrowsArgumentException_ForViewTypeOfPrivateType()
            {
                // Arrange
                var viewType = typeof(IMyPrivateView);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
            }
#endif

#if NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void ThrowsArgumentException_ForViewTypeContainingPublicMethods()
            {
                // Arrange
                var viewType = typeof(IMyViewWithMethod);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
            }
#endif

#if NO_INTERNALS_VISIBLE_TO
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
#endif

#if NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void Passes_ForViewTypeInheritingIView()
            {
                // Arrange
                var viewType = typeof(IMyView1);

                // Act
                CompositeViewTypeResolver.ValidateViewType(viewType);

                // Assert
                Assert.True(true);
            }
#endif

#if NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void Passes_ForViewTypeInheritingGenericIView()
            {
                // Arrange
                var viewType = typeof(IMyView2);

                // Act
                CompositeViewTypeResolver.ValidateViewType(viewType);

                // Assert
                Assert.True(true);
            }
#endif

#if NO_INTERNALS_VISIBLE_TO
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
#endif

#if NO_INTERNALS_VISIBLE_TO
            [Fact]
            public static void Passes_ForViewTypeContainingPropertiesAndEventHandlers()
            {
                // Arrange
                var viewType = typeof(IMyView);

                // Act
                CompositeViewTypeResolver.ValidateViewType(viewType);

                // Assert
                Assert.True(true);
            }
#endif
        }

        #region Helper classes

        public interface IMyView1 : IView { }

        public interface IMyView2 : IView<Object> { }

        public interface IMyView : IView
        {
            event EventHandler MyHandler;

            string MyProperty { get; set; }
        }

        public interface IMyViewWithMethod : IView
        {
            void MyMethod();
        }

        interface IMyPrivateView : IView { }

        #endregion
    }
}
