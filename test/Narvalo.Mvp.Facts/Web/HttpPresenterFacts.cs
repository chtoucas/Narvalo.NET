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
        public static class Ctor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new StubPresenter(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new StubPresenterOf(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new StubPresenterView(view: null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new StubPresenterViewWithModel(view: null));
            }

            [Fact]
            public static void InitializesViewModel_WhenIViewWithModel()
            {
                // Arrange
                var view = new StubIViewWithModel();

                // Act
                new StubPresenterOf(view);

                // Assert
                Assert.NotNull(view.Model);
            }

            [Fact]
            public static void InitializesViewModel_WhenViewWithModel()
            {
                // Arrange
                var view = new StubViewWithModel();

                // Act
                var presenter = new StubPresenterViewWithModel(view);

                // Assert
                Assert.NotNull(view.Model);
            }
        }

        public static class ViewProperty
        {
            [Fact]
            public static void IsSetCorrectly_WhenIView()
            {
                // Arrange
                var view = new Mock<IView>().Object;

                // Act
                var presenter = new StubPresenter(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenIViewWithModel()
            {
                // Arrange
                var view = new Mock<IView<StubViewModel>>().Object;

                // Act
                var presenter = new StubPresenterOf(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenView()
            {
                // Arrange
                var view = new Mock<IStubView>().Object;

                // Act
                var presenter = new StubPresenterView(view);

                // Assert
                Assert.Same(view, presenter.View);
            }

            [Fact]
            public static void IsSetCorrectly_WhenViewWithModel()
            {
                // Arrange
                var view = new Mock<IStubViewWithModel>().Object;

                // Act
                var presenter = new StubPresenterViewWithModel(view);

                // Assert
                Assert.Same(view, presenter.View);
            }
        }

        public static class HttpContextProperty
        {
            [Fact]
            public static void ReturnsAmbientHttpContext()
            {
                // Arrange
                var view = new Mock<IView>().Object;
                var httpContext = new Mock<HttpContextBase>().Object;

                // Act
                var presenter = new StubPresenter(view);
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
                var view = new Mock<IView>().Object;
                var mockHttpContext = new Mock<HttpContextBase>();
                var cache = new Cache();
                mockHttpContext.Setup(h => h.Cache).Returns(cache);
                var httpContext = mockHttpContext.Object;

                // Act
                var presenter = new StubPresenter(view);
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
                var view = new Mock<IView>().Object;
                var mockHttpContext = new Mock<HttpContextBase>();
                var request = new Mock<HttpRequestBase>().Object;
                mockHttpContext.Setup(h => h.Request).Returns(request);
                var httpContext = mockHttpContext.Object;

                // Act
                var presenter = new StubPresenter(view);
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
                var view = new Mock<IView>().Object;
                var mockHttpContext = new Mock<HttpContextBase>();
                var response = new Mock<HttpResponseBase>().Object;
                mockHttpContext.Setup(h => h.Response).Returns(response);
                var httpContext = mockHttpContext.Object;

                // Act
                var presenter = new StubPresenter(view);
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
                var view = new Mock<IView>().Object;
                var mockHttpContext = new Mock<HttpContextBase>();
                var server = new Mock<HttpServerUtilityBase>().Object;
                mockHttpContext.Setup(h => h.Server).Returns(server);
                var httpContext = mockHttpContext.Object;

                // Act
                var presenter = new StubPresenter(view);
                (presenter as Internal.IHttpPresenter).HttpContext = httpContext;

                // Assert
                Assert.Same(server, presenter.Server);
            }
        }

        #region Stubs

        public interface IStubView : IView
        {
            event EventHandler TestHandler;
        }

        public interface IStubViewWithModel : IView<StubViewModel>
        {
            event EventHandler TestHandler;
        }

        public class StubViewModel { }

        class StubIViewWithModel : IView<StubViewModel>
        {
            event EventHandler IView.Load
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }

            public StubViewModel Model { get; set; }

            public bool ThrowIfNoPresenterBound
            {
                get { throw new NotImplementedException(); }
            }
        }

        class StubViewWithModel : IStubViewWithModel
        {
            event EventHandler IView.Load
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }

            event EventHandler IStubViewWithModel.TestHandler
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }

            public StubViewModel Model { get; set; }

            public bool ThrowIfNoPresenterBound
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }

        class StubPresenter : HttpPresenter<IView>
        {
            public StubPresenter(IView view) : base(view) { }
        }

        class StubPresenterOf : HttpPresenterOf<StubViewModel>
        {
            public StubPresenterOf(IView<StubViewModel> view) : base(view) { }
        }

        class StubPresenterView : HttpPresenter<IStubView>
        {
            public StubPresenterView(IStubView view) : base(view) { }
        }

        class StubPresenterViewWithModel : HttpPresenter<IStubViewWithModel, StubViewModel>
        {
            public StubPresenterViewWithModel(IStubViewWithModel view) : base(view) { }
        }

        #endregion
    }
}