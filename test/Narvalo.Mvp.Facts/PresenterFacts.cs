// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using NSubstitute;
    using Xunit;

    public static partial class PresenterFacts
    {
        public static class Ctor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyPresenter(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_ForPresenterOf()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyPresenterOf(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_ForPresenter()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyPresenterWithModel(view: null));
            }

            [Fact]
            public static void InitializesViewModel_ForPresenterOf()
            {
                // Arrange
                var view = Substitute.For<IView<MyViewModel>>();

                // Act
                new MyPresenterOf(view);

                // Assert
                Assert.NotNull(view.Model);
            }

            [Fact]
            public static void InitializesViewModel_ForPresenter()
            {
                // Arrange
                var view = Substitute.For<IMyViewWithModel>();

                // Act
                new MyPresenterWithModel(view);

                // Assert
                Assert.NotNull(view.Model);
            }
        }

        public static class ViewProperty
        {
            [Fact]
            public static void IsSetCorrectly()
            {
                // Arrange
                var view = Substitute.For<IMyView>();

                // Act
                var presenter = new MyPresenter(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_ForPresenterOf()
            {
                // Arrange
                var view = Substitute.For<IView<MyViewModel>>();

                // Act
                var presenter = new MyPresenterOf(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_ForPresenter()
            {
                // Arrange
                var view = Substitute.For<IMyViewWithModel>();

                // Act
                var presenter = new MyPresenterWithModel(view);

                // Assert
                Assert.Same(view, presenter.View);
            }
        }

        public interface IMyView : IView
        {
            event EventHandler TestHandler;
        }

        public interface IMyViewWithModel : IView<MyViewModel>
        {
            event EventHandler TestHandler;
        }

        public class MyViewModel { }

        class MyPresenter : Presenter<IMyView>
        {
            public MyPresenter(IMyView view) : base(view) { }
        }

        class MyPresenterOf : PresenterOf<MyViewModel>
        {
            public MyPresenterOf(IView<MyViewModel> view) : base(view) { }
        }

        class MyPresenterWithModel : Presenter<IMyViewWithModel, MyViewModel>
        {
            public MyPresenterWithModel(IMyViewWithModel view) : base(view) { }
        }
    }
}