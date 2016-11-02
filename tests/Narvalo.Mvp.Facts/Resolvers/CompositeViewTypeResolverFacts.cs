// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    using Xunit;

    public static partial class CompositeViewTypeResolverFacts
    {
        #region Resolve()

        [Fact]
        public static void Resolve_ThrowsArgumentNullException_ForNullViewType()
        {
            // Arrange
            var resolver = new CompositeViewTypeResolver();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => resolver.Resolve(viewType: null));
        }

        #endregion
    }

    public static partial class CompositeViewTypeResolverFacts
    {
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

        private interface IMyPrivateView_ : IView { }
    }

#if !NO_INTERNALS_VISIBLE_TO // White-box tests.

    public static partial class CompositeViewTypeResolverFacts
    {
        #region ValidateViewType()

        [Fact]
        public static void ValidateViewType_ThrowsArgumentException_ForViewTypeOfClassType()
        {
            // Arrange
            var viewType = typeof(Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
        }

        [Fact]
        public static void ValidateViewType_ThrowsArgumentException_ForViewTypeNotInheritingIView()
        {
            // Arrange
            var viewType = typeof(IDisposable);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
        }

        [Fact]
        public static void ValidateViewType_ThrowsArgumentException_ForViewTypeOfPrivateType()
        {
            // Arrange
            var viewType = typeof(IMyPrivateView_);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
        }

        [Fact]
        public static void ValidateViewType_ThrowsArgumentException_ForViewTypeContainingPublicMethods()
        {
            // Arrange
            var viewType = typeof(IMyViewWithMethod);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CompositeViewTypeResolver.ValidateViewType(viewType));
        }

        [Fact]
        public static void ValidateViewType_Passes_ForViewTypeOfIViewType()
        {
            // Arrange
            var viewType = typeof(IView);

            // Act
            CompositeViewTypeResolver.ValidateViewType(viewType);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void ValidateViewType_Passes_ForViewTypeInheritingIView()
        {
            // Arrange
            var viewType = typeof(IMyView1);

            // Act
            CompositeViewTypeResolver.ValidateViewType(viewType);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void ValidateViewType_Passes_ForViewTypeInheritingGenericIView()
        {
            // Arrange
            var viewType = typeof(IMyView2);

            // Act
            CompositeViewTypeResolver.ValidateViewType(viewType);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void ValidateViewType_Passes_ForViewTypeOfGenericIViewType()
        {
            // Arrange
            var viewType = typeof(IView<Object>);

            // Act
            CompositeViewTypeResolver.ValidateViewType(viewType);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public static void ValidateViewType_Passes_ForViewTypeContainingPropertiesAndEventHandlers()
        {
            // Arrange
            var viewType = typeof(IMyView);

            // Act
            CompositeViewTypeResolver.ValidateViewType(viewType);

            // Assert
            Assert.True(true);
        }

        #endregion
    }

#endif
}
