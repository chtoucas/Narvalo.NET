// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using Moq;
    using Xunit;

    public static class PresenterFactoryFacts
    {
        class DisposablePresenter : Presenter<IView>, IDisposable
        {
            public DisposablePresenter(IView view) : base(view) { }

            public bool DisposeCalled { get; private set; }

            public void Dispose()
            {
                DisposeCalled = true;
            }
        }

        public class ErrorPresenter : Presenter<IView>
        {
            public ErrorPresenter(IView view)
                : base(view)
            {
                throw new ApplicationException("test exception");
            }
        }

        public static class TheReleaseMethod
        {
            [Fact]
            public static void DefaultPresenterFactory_DisposesPresenter()
            {
                // Arrange
                var factory = new PresenterFactory();
                var view = new Mock<IView>().Object;
                var presenter = new DisposablePresenter(view);

                // Act
                factory.Release(presenter);

                // Assert
                Assert.True(presenter.DisposeCalled);
            }
        }

        public static class TheCreateMethod
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullPresenterType()
            {
                // Arrange
                var factory = new PresenterFactory();
                var viewType = typeof(IView);
                var view = new Mock<IView>().Object;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => factory.Create(null, viewType, view));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullViewType()
            {
                // Arrange
                var factory = new PresenterFactory();
                var presenterType = typeof(Presenter<IView>);
                var view = new Mock<IView>().Object;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => factory.Create(presenterType, null, view));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView()
            {
                // Arrange
                var factory = new PresenterFactory();
                var presenterType = typeof(Presenter<IView>);
                var viewType = typeof(IView);

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => factory.Create(presenterType, viewType, null));
            }

            [Fact]
            public static void ReturnsInstance()
            {
                // Arrange
                var factory = new PresenterFactory();
                var presenterType = typeof(Stubs.PresenterForIView);
                var viewType = typeof(IView);
                var view = new Mock<IView>().Object;

                // Act
                var presenter = factory.Create(
                    presenterType,
                    viewType,
                    view);

                // Assert
                Assert.True(presenter is Stubs.PresenterForIView);
            }

            [Fact]
            public static void ThrowsPresenterBindingException()
            {
                // Arrange
                var factory = new PresenterFactory();
                var presenterType = typeof(ErrorPresenter);
                var viewType = typeof(IView);
                var view = new Mock<IView>().Object;

                // Act & Assert
                Assert.Throws<PresenterBindingException>(() => factory.Create(presenterType, viewType, view));
            }

            [Fact]
            public static void WrapsOriginalException()
            {
                // Arrange
                var factory = new PresenterFactory();
                var presenterType = typeof(ErrorPresenter);
                var viewType = typeof(IView);
                var view = new Mock<IView>().Object;

                try {
                    // Act
                    factory.Create(presenterType, viewType, view);
                }
                catch (Exception ex) {
                    // Assert
                    Assert.True(ex.InnerException is ApplicationException);
                }
            }
        }
    }
}
