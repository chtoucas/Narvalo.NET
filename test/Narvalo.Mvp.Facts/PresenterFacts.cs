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
            public static void ThrowsArgumentNullException_ForNullView_WithPresenterT1()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyPresenter<IMyView>(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WithPresenterOf()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyPresenterOf<MyViewModel>(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WithPresenterT2()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyPresenter<IMyViewWithModel, MyViewModel>(view: null));
            }

            [Fact]
            public static void InitializesViewModel_WithPresenterOf()
            {
                // Arrange
                var view = Substitute.For<IView<MyViewModel>>();

                // Act
                new MyPresenterOf<MyViewModel>(view);

                // Assert
                Assert.NotNull(view.Model);
            }

            [Fact]
            public static void InitializesViewModel_WithPresenterT2()
            {
                // Arrange
                var view = Substitute.For<IMyViewWithModel>();

                // Act
                new MyPresenter<IMyViewWithModel, MyViewModel>(view);

                // Assert
                Assert.NotNull(view.Model);
            }
        }

        public static class ViewProperty
        {
            [Fact]
            public static void IsSetCorrectly_WithPresenterT1()
            {
                // Arrange
                var view = Substitute.For<IMyView>();

                // Act
                var presenter = new MyPresenter<IMyView>(view);

                // Assert
                Assert.Equal(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WithPresenterOf()
            {
                // Arrange
                var view = Substitute.For<IView<MyViewModel>>();

                // Act
                var presenter = new MyPresenterOf<MyViewModel>(view);

                // Assert
                Assert.Equal(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WithPresenterT2()
            {
                // Arrange
                var view = Substitute.For<IMyViewWithModel>();

                // Act
                var presenter = new MyPresenter<IMyViewWithModel, MyViewModel>(view);

                // Assert
                Assert.Equal(view, presenter.View);
            }
        }

        #region Supporting classes

        public interface IMyView : IView
        {
            event EventHandler TestHandler;
        }

        public interface IMyViewWithModel : IView<MyViewModel>
        {
            event EventHandler TestHandler;
        }

        public class MyViewModel { }

        class MyPresenter<T> : Presenter<T> where T : class, IView
        {
            public MyPresenter(T view) : base(view) { }
        }

        class MyPresenterOf<T> : PresenterOf<T> where T : class, new()
        {
            public MyPresenterOf(IView<T> view) : base(view) { }
        }

        class MyPresenter<T1, T2> : Presenter<T1, T2>
            where T1 : class, IView<T2>
            where T2 : class, new()
        {
            public MyPresenter(T1 view) : base(view) { }
        }

        #endregion
    }
}