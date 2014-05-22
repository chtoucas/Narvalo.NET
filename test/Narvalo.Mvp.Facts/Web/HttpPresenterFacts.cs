// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
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
                Assert.Throws<ArgumentNullException>(() => new Stubs.HttpPresenterForIView(null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenIViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.HttpPresenterForIViewWithModel(null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenView()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.HttpPresenterForView(null));
            }

            [Fact]
            public static void ThrowsArgumentNullException_ForNullView_WhenViewWithModel()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new Stubs.HttpPresenterForViewWithModel(null));
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
    }

    //[Fact]
    //public void Presenter_Cache_ReturnsCacheFromHttpContext()
    //{
    //    // Arrange
    //    var view = MockRepository.GenerateStub<IView>();
    //    var httpContext = MockRepository.GenerateStub<HttpContextBase>();
    //    var cache = new Cache();
    //    httpContext.Stub(h => h.Cache).Return(cache);

    //    // Act
    //    var presenter = new TestPresenter(view) { HttpContext = httpContext };

    //    // Assert
    //    Assert.Same(cache, presenter.Cache);
    //}

    //[Fact]
    //public void Presenter_Request_ReturnsRequestFromHttpContext()
    //{
    //    // Arrange
    //    var view = MockRepository.GenerateStub<IView>();
    //    var httpContext = MockRepository.GenerateStub<HttpContextBase>();
    //    var request = MockRepository.GenerateStub<HttpRequestBase>();
    //    httpContext.Stub(h => h.Request).Return(request);

    //    // Act
    //    var presenter = new TestPresenter(view) { HttpContext = httpContext };

    //    // Assert
    //    Assert.Same(request, presenter.Request);
    //}

    //[Fact]
    //public void Presenter_Response_ReturnsResponseFromHttpContext()
    //{
    //    // Arrange
    //    var view = MockRepository.GenerateStub<IView>();
    //    var httpContext = MockRepository.GenerateStub<HttpContextBase>();
    //    var response = MockRepository.GenerateStub<HttpResponseBase>();
    //    httpContext.Stub(h => h.Response).Return(response);

    //    // Act
    //    var presenter = new TestPresenter(view) {HttpContext = httpContext};

    //    // Assert
    //    Assert.Same(response, presenter.Response);
    //}

    //[Fact]
    //public void Presenter_Server_ReturnsServerFromHttpContext()
    //{
    //    // Arrange
    //    var view = MockRepository.GenerateStub<IView>();
    //    var httpContext = MockRepository.GenerateStub<HttpContextBase>();
    //    var server = MockRepository.GenerateStub<HttpServerUtilityBase>();
    //    httpContext.Stub(h => h.Server).Return(server);

    //    // Act
    //    var presenter = new TestPresenter(view) {HttpContext = httpContext};

    //    // Assert
    //    Assert.Same(server, presenter.Server);
    //}

    //[Fact]
    //public void Presenter_RouteData_ReturnsRouteDataFromHttpContext()
    //{
    //    // Arrange
    //    var view = MockRepository.GenerateStub<IView>();
    //    var httpContext = MockRepository.GenerateStub<HttpContextBase>();
    //    var request = MockRepository.GenerateStub<HttpRequestBase>();
    //    httpContext.Stub(h => h.Request).Return(request);
    //    var route = MockRepository.GenerateStub<RouteBase>();
    //    var routeData = new RouteData();
    //    routeData.Values.Add("TestRouteDataValue", 1);
    //    route.Stub(r => r.GetRouteData(httpContext)).Return(routeData);
    //    RouteTable.Routes.Add("Test Route", route);

    //    // Act
    //    var presenter = new TestPresenter(view) { HttpContext = httpContext };

    //    // Assert
    //    Assert.AreEqual(1, presenter.RouteData.Values["TestRouteDataValue"]);
    //}
}