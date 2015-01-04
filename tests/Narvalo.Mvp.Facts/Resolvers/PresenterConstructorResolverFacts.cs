// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using NSubstitute;
    using Xunit;

    public static class PresenterConstructorResolverFacts
    {
        public static class ResolveMethod
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullPresenterType()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var viewType = typeof(IView);

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => resolver.Resolve(null, viewType));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullViewType()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var presenterType = typeof(IPresenter<IView>);

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => resolver.Resolve(presenterType, null));
            }

            [Fact]
            public static void ThrowsArgumentException_ForViewTypeMismatch()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var presenterType = typeof(MyPresenter<IMyView1>);
                var viewType = typeof(IMyView2);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => resolver.Resolve(presenterType, viewType));
            }

            [Fact]
            public static void ThrowsArgumentException_WhenMissingRequiredConstructor()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var presenterType = typeof(MyBadPresenter<IMyView1>);
                var viewType = typeof(IMyView1);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => resolver.Resolve(presenterType, viewType));
            }

            [Fact]
            public static void ThrowsArgumentException_WhenPresenterTypeIsPrivate()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var presenterType = typeof(MyPrivatePresenter);
                var viewType = typeof(IView);

                // Act & Assert
                Assert.Throws<ArgumentException>(() => resolver.Resolve(presenterType, viewType));
            }

            [Fact]
            public static void ReturnsExpectedConstructor()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();
                var presenterType = typeof(MyPresenter<IMyView1>);
                var viewType = typeof(IMyView1);
                var view = Substitute.For<IMyView1>();

                // Act
                var ctor = resolver.Resolve(presenterType, viewType);
                var instance = ctor.Invoke(null, new[] { view });
                var presenter = instance as MyPresenter<IMyView1>;

                // Assert
                Assert.True(presenter != null);

                if (presenter != null) {
                    Assert.Equal(view, presenter.View);
                }
            }

            [Fact]
            public static void ReturnsDifferentConstructors_ForDifferentViewTypes()
            {
                // Arrange
                var resolver = new PresenterConstructorResolver();

                // Act
                var ctor1 = resolver.Resolve(typeof(MyPresenter<IView>), typeof(IMyView3));
                var ctor2 = resolver.Resolve(typeof(MyPresenter<IView>), typeof(IMyView4));

                // Assert
                Assert.NotEqual(ctor1, ctor2);
            }
        }

        #region Helper classes

        public interface IMyView1 : IView<String> { }

        public interface IMyView2 : IView<Int16> { }

        public interface IMyView3 : IView<Int32> { }

        public interface IMyView4 : IView<Int64> { }

        public class MyPresenter<T> : IPresenter<T> where T : IView
        {
            public MyPresenter(T view)
            {
                View = view;
            }

            public MyPresenter(IMyView3 view) { }

            public MyPresenter(IMyView4 view) { }

            public T View { get; private set; }

            public IMessageCoordinator Messages
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class MyBadPresenter<T> : IPresenter<T> where T : IView
        {
            public MyBadPresenter() { }

            public MyBadPresenter(int param) { }

            public MyBadPresenter(T view, int param) { }

            public T View
            {
                get { throw new NotImplementedException(); }
            }

            public IMessageCoordinator Messages
            {
                get { throw new NotImplementedException(); }
            }
        }

        class MyPrivatePresenter : IPresenter<IView>
        {
            public IView View
            {
                get { throw new NotImplementedException(); }
            }

            public IMessageCoordinator Messages
            {
                get { throw new NotImplementedException(); }
            }
        }

        #endregion
    }
}
