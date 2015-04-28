// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;

    using NSubstitute;
    using Xunit;

    public static partial class PresenterFactoryFacts
    {
        #region Create()

        [Fact]
        public static void Create_ThrowsArgumentNullException_ForNullPresenterType()
        {
            // Arrange
            var factory = new PresenterFactory();
            var viewType = typeof(IView);
            var view = Substitute.For<IView>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => factory.Create(null, viewType, view));
        }

        [Fact]
        public static void Create_ThrowsArgumentNullException_ForNullViewType()
        {
            // Arrange
            var factory = new PresenterFactory();
            var presenterType = typeof(IPresenter<IView>);
            var view = Substitute.For<IView>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => factory.Create(presenterType, null, view));
        }

        [Fact]
        public static void Create_ThrowsArgumentNullException_ForNullView()
        {
            // Arrange
            var factory = new PresenterFactory();
            var presenterType = typeof(IPresenter<IView>);
            var viewType = typeof(IView);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => factory.Create(presenterType, viewType, null));
        }

        [Fact]
        public static void Create_ReturnsInstance()
        {
            // Arrange
            var factory = new PresenterFactory();
            var presenterType = typeof(MyPresenter);
            var viewType = typeof(IView);
            var view = Substitute.For<IView>();

            // Act
            var presenter = factory.Create(
                presenterType,
                viewType,
                view);

            // Assert
            Assert.True(presenter is MyPresenter);
        }

        [Fact]
        public static void Create_ThrowsPresenterBindingException_WhenConstructorThrows()
        {
            // Arrange
            var factory = new PresenterFactory();
            var presenterType = typeof(MyErrorPresenter);
            var viewType = typeof(IView);
            var view = Substitute.For<IView>();

            // Act & Assert
            Assert.Throws<PresenterBindingException>(() => factory.Create(presenterType, viewType, view));
        }

        [Fact]
        public static void Create_WrapsOriginalException_WhenConstructorThrows()
        {
            // Arrange
            var factory = new PresenterFactory();
            var presenterType = typeof(MyErrorPresenter);
            var viewType = typeof(IView);
            var view = Substitute.For<IView>();

            try
            {
                // Act
                factory.Create(presenterType, viewType, view);
            }
            catch (PresenterBindingException ex)
            {
                // Assert
                Assert.True(ex.InnerException is ApplicationException);
            }
        }

        #endregion

        #region Release()

        [Fact]
        public static void Release_DisposesPresenter()
        {
            // Arrange
            var factory = new PresenterFactory();
            var view = Substitute.For<IView>();

            using (var presenter = new MyDisposablePresenter(view))
            {
                // Act
                factory.Release(presenter);

                // Assert
                Assert.True(presenter.DisposeWasCalled);
            }
        }

        #endregion
    }

    public static partial class PresenterFactoryFacts
    {
        public sealed class MyPresenter : Presenter<IView>
        {
            public MyPresenter(IView view) : base(view) { }
        }

        public sealed class MyDisposablePresenter : Presenter<IView>, IDisposable
        {
            public MyDisposablePresenter(IView view) : base(view) { }

            public bool DisposeWasCalled { get; private set; }

            public void Dispose()
            {
                DisposeWasCalled = true;
            }
        }

        public sealed class MyErrorPresenter : Presenter<IView>
        {
            public MyErrorPresenter(IView view)
                : base(view)
            {
                throw new ApplicationException("test exception");
            }
        }
    }
}
