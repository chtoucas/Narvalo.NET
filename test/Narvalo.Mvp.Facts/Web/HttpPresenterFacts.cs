// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using System.Web.Caching;
    using Moq;
    using Xunit;

    public static partial class HttpPresenterFacts
    {
        public static class TheConstructor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.HttpPresenterForIView(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.HttpPresenterForIViewWithModel(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.HttpPresenterForView(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.HttpPresenterForViewWithModel(view: null));
            }

            [Fact]
            public static void InitializesViewModel_WhenIViewWithModel()
            {
                // Arrange
                var view = new Stubs.ViewWithModel();

                // Act
                new Stubs.HttpPresenterForIViewWithModel(view);

                // Assert
                Assert.NotNull(view.Model);
            }

            [Fact]
            public static void InitializesViewModel_WhenViewWithModel()
            {
                // Arrange
                var view = new Stubs.SimpleViewWithModel();

                // Act
                var presenter = new Stubs.HttpPresenterForViewWithModel(view);

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
                var presenter = new Stubs.HttpPresenterForIView(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenIViewWithModel()
            {
                // Arrange
                var view = new Mock<IView<Stubs.ViewModel>>().Object;

                // Act
                var presenter = new Stubs.HttpPresenterForIViewWithModel(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenView()
            {
                // Arrange
                var view = new Mock<Stubs.ISimpleView>().Object;

                // Act
                var presenter = new Stubs.HttpPresenterForView(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenViewWithModel()
            {
                // Arrange
                var view = new Mock<Stubs.ISimpleViewWithModel>().Object;

                // Act
                var presenter = new Stubs.HttpPresenterForViewWithModel(view);

                // Assert
                Assert.Same(view, presenter.View);
            }
        }

        public static class TheHttpContextProperty
        {
            [Fact]
            public static void ReturnsAmbientHttpContext()
            {
                // Arrange
                var view = new Mock<IView>().Object;
                var httpContext = new Mock<HttpContextBase>().Object;

                // Act
                var presenter = new Stubs.HttpPresenterForIView(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(httpContext, presenter.HttpContext);
            }
        }

        public static class TheCacheProperty
        {
            [Fact]
            public static void ReturnsCacheFromHttpContext()
            {
                // Arrange
                var view = new Mock<IView>().Object;
                var mockHttpContext = new Mock<HttpContextBase>();
                var cache = new Cache();
                mockHttpContext.Setup(h => h.Cache).Returns(cache);
                var httpContext = mockHttpContext.Object;

                // Act
                var presenter = new Stubs.HttpPresenterForIView(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(cache, presenter.Cache);
            }
        }

        public static class TheRequestProperty
        {
            [Fact]
            public static void ReturnsRequestFromHttpContext()
            {
                // Arrange
                var view = new Mock<IView>().Object;
                var mockHttpContext = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>().Object;
                mockHttpContext.Setup(h => h.Request).Returns(request);
                var httpContext = mockHttpContext.Object;

                // Act
                var presenter = new Stubs.HttpPresenterForIView(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(request, presenter.Request);
            }
        }

        public static class TheResponseProperty
        {
            [Fact]
            public static void ReturnsResponseFromHttpContext()
            {
                // Arrange
                var view = new Mock<IView>().Object;
                var mockHttpContext = new Mock<HttpContextBase>();
                var response = new Mock<HttpResponseBase>().Object;
                mockHttpContext.Setup(h => h.Response).Returns(response);
                var httpContext = mockHttpContext.Object;

                // Act
                var presenter = new Stubs.HttpPresenterForIView(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(response, presenter.Response);
            }
        }

        public static class TheServerProperty
        {
            [Fact]
            public static void ReturnsServerFromHttpContext()
            {
                // Arrange
                var view = new Mock<IView>().Object;
                var mockHttpContext = new Mock<HttpContextBase>();
                var server = new Mock<HttpServerUtilityBase>().Object;
                mockHttpContext.Setup(h => h.Server).Returns(server);
                var httpContext = mockHttpContext.Object;

                // Act
                var presenter = new Stubs.HttpPresenterForIView(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(server, presenter.Server);
            }
        }
    }
}