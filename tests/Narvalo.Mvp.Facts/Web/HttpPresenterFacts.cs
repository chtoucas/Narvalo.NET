// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using System.Web.Caching;
    using Narvalo.Mvp;
    using NSubstitute;
    using Xunit;

    public static partial class HttpPresenterFacts
    {
        public static class Ctor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyHttpPresenter(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyHttpPresenterOf(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new MyHttpPresenterWithModel(view: null));
            }

            [Fact]
            public static void InitializesViewModel_WhenIViewWithModel()
            {
                // Arrange
                var view = Substitute.For<IView<MyViewModel>>();

                // Act
                new MyHttpPresenterOf(view);

                // Assert
                Assert.NotNull(view.Model);
            }

            [Fact]
            public static void InitializesViewModel_WhenViewWithModel()
            {
                // Arrange
                var view = Substitute.For<IMyViewWithModel>();

                // Act
                new MyHttpPresenterWithModel(view);

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
                var presenter = new MyHttpPresenter(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenIViewWithModel()
            {
                // Arrange
                var view = Substitute.For<IView<MyViewModel>>();

                // Act
                var presenter = new MyHttpPresenterOf(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenViewWithModel()
            {
                // Arrange
                var view = Substitute.For<IMyViewWithModel>();

                // Act
                var presenter = new MyHttpPresenterWithModel(view);

                // Assert
                Assert.Same(view, presenter.View);
            }
        }

        #region Helper classes

        public interface IMyView : IView
        {
            event EventHandler TestHandler;
        }

        public interface IMyViewWithModel : IView<MyViewModel>
        {
            event EventHandler TestHandler;
        }

        public class MyViewModel { }

        public class MyHttpPresenter : HttpPresenter<IMyView>
        {
            public MyHttpPresenter(IMyView view) : base(view) { }
        }

        public class MyHttpPresenterOf : HttpPresenterOf<MyViewModel>
        {
            public MyHttpPresenterOf(IView<MyViewModel> view) : base(view) { }
        }

        public class MyHttpPresenterWithModel : HttpPresenter<IMyViewWithModel, MyViewModel>
        {
            public MyHttpPresenterWithModel(IMyViewWithModel view) : base(view) { }
        }

        #endregion
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class HttpPresenterFacts
    {
        public static class HttpContextProperty
        {
            [Fact]
            public static void ReturnsAmbientHttpContext()
            {
                // Arrange
                var view = Substitute.For<IMyView>();
                var httpContext = Substitute.For<HttpContextBase>();

                // Act
                var presenter = new MyHttpPresenter(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(httpContext, presenter.HttpContext);
            }
        }

        public static class CacheProperty
        {
            [Fact]
            public static void ReturnsCacheFromHttpContext()
            {
                // Arrange
                var view = Substitute.For<IMyView>();
                var cache = new Cache();
                var httpContext = Substitute.For<HttpContextBase>();
                httpContext.Cache.Returns(cache);

                // Act
                var presenter = new MyHttpPresenter(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(cache, presenter.Cache);
            }
        }

        public static class RequestProperty
        {
            [Fact]
            public static void ReturnsRequestFromHttpContext()
            {
                // Arrange
                var view = Substitute.For<IMyView>();
                var request = Substitute.For<HttpRequestBase>();
                var httpContext = Substitute.For<HttpContextBase>();
                httpContext.Request.Returns(request);

                // Act
                var presenter = new MyHttpPresenter(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(request, presenter.Request);
            }
        }

        public static class ResponseProperty
        {
            [Fact]
            public static void ReturnsResponseFromHttpContext()
            {
                // Arrange
                var view = Substitute.For<IMyView>();
                var response = Substitute.For<HttpResponseBase>();
                var httpContext = Substitute.For<HttpContextBase>();
                httpContext.Response.Returns(response);

                // Act
                var presenter = new MyHttpPresenter(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(response, presenter.Response);
            }
        }

        public static class ServerProperty
        {
            [Fact]
            public static void ReturnsServerFromHttpContext()
            {
                // Arrange
                var view = Substitute.For<IMyView>();
                var server = Substitute.For<HttpServerUtilityBase>();
                var httpContext = Substitute.For<HttpContextBase>();
                httpContext.Server.Returns(server);

                // Act
                var presenter = new MyHttpPresenter(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(server, presenter.Server);
            }
        }
    }

#endif
}