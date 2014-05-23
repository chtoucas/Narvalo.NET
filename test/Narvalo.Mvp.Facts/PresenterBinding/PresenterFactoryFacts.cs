// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using Moq;
    using Xunit;

    public static class PresenterFactoryFacts
    {
        public static class CreateMethod
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
                var presenterType = typeof(StubPresenter);
                var viewType = typeof(IView);
                var view = new Mock<IView>().Object;

                // Act
                var presenter = factory.Create(
                    presenterType,
                    viewType,
                    view);

                // Assert
                Assert.True(presenter is StubPresenter);
            }

            [Fact]
            public static void ThrowsPresenterBindingException_WhenBindingFails()
            {
                // Arrange
                var factory = new PresenterFactory();
                var presenterType = typeof(StubErrorPresenter);
                var viewType = typeof(IView);
                var view = new Mock<IView>().Object;

                // Act & Assert
                Assert.Throws<PresenterBindingException>(() => factory.Create(presenterType, viewType, view));
            }

            [Fact]
            public static void WrapsOriginalException_WhenBindingFails()
            {
                // Arrange
                var factory = new PresenterFactory();
                var presenterType = typeof(StubErrorPresenter);
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

        public static class ReleaseMethod
        {
            [Fact]
            public static void DisposesPresenter()
            {
                // Arrange
                var factory = new PresenterFactory();
                var view = new Mock<IView>().Object;
                var presenter = new StubDisposablePresenter(view);

                // Act
                factory.Release(presenter);

                // Assert
                Assert.True(presenter.DisposeCalled);
            }
        }

        #region Stubs

        // NB: Keep these classes public, otherwise "PresenterFactory" can not introspect them.

        public class StubPresenter : Presenter<IView>
        {
            public StubPresenter(IView view) : base(view) { }
        }

        public class StubDisposablePresenter : Presenter<IView>, IDisposable
        {
            public StubDisposablePresenter(IView view) : base(view) { }

            public bool DisposeCalled { get; private set; }

            public void Dispose()
            {
                DisposeCalled = true;
            }
        }

        public class StubErrorPresenter : Presenter<IView>
        {
            public StubErrorPresenter(IView view)
                : base(view)
            {
                throw new ApplicationException("test exception");
            }
        }

        class StubPrivatePresenter : Presenter<IView>
        {
            public StubPrivatePresenter(IView view) : base(view) { }
        }

        #endregion
    }
}
