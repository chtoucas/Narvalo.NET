// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using Moq;
    using Xunit;

    public static class PresenterFacts
    {
        public static class TheConstructor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.PresenterForIView(null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.PresenterForIViewWithModel(null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.PresenterForView(null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.PresenterForViewWithModel(null));
            }

            [Fact]
            public static void InitializesViewModel_WhenIViewWithModel()
            {
                // Arrange
                var view = new Stubs.ViewWithModel();
                // Act
                new Stubs.PresenterForIViewWithModel(view);
                // Assert
                Assert.NotNull(view.Model);
            }

            [Fact]
            public static void InitializesViewModel_WhenViewWithModel()
            {
                // Arrange
                var view = new Stubs.SimpleViewWithModel();
                // Act
                var presenter = new Stubs.PresenterForViewWithModel(view);
                // Assert
                Assert.NotNull(view.Model);
            }
        }

        public static class TheViewProperty
        {
            [Fact]
            public static void IsSetCorrectly_WhenIView()
            {
                // Arrange
                var view = new Mock<IView>().Object;
                // Act
                var presenter = new Stubs.PresenterForIView(view);
                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenIViewWithModel()
            {
                // Arrange
                var view = new Mock<IView<Stubs.ViewModel>>().Object;
                // Act
                var presenter = new Stubs.PresenterForIViewWithModel(view);
                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenView()
            {
                // Arrange
                var view = new Mock<Stubs.ISimpleView>().Object;
                // Act
                var presenter = new Stubs.PresenterForView(view);
                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenViewWithModel()
            {
                // Arrange
                var view = new Mock<Stubs.ISimpleViewWithModel>().Object;
                // Act
                var presenter = new Stubs.PresenterForViewWithModel(view);
                // Assert
                Assert.Same(view, presenter.View);
            }
        }
    }
}