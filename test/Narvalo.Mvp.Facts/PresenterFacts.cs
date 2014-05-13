namespace Narvalo.Mvp
{
    using System;
    using Xunit;

    public static class PresenterFacts
    {
        public static class TheConstructor
        {
            [Fact]
            public static void InitializesDefaultViewModelForViewTypesThatImplementIViewTModel()
            {
                // Arrange
                var view = new TestViewWithModel();
                // Act
                new TestPresenterWithModelBasedView(view);
                // Assert
                Assert.Null(view.Model);
            }
        }

        //[Fact]
        //public void Presenter_Constructor_ShouldSupportNonModelBasedViews()
        //{
        //    // Arrange
        //    IView view = null; // MockRepository.GenerateMock<IView>();

        //    // Act
        //    new TestPresenter(view);

        //    // Assert
        //}

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

        class TestModel { }

        class TestViewWithModel : IView<TestModel>
        {
            event EventHandler IView.Load
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }

            public TestModel Model { get; set; }

            public bool ThrowIfNoPresenterBound
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }

        class TestPresenterWithModelBasedView
            : Presenter<TestViewWithModel>
        {
            public TestPresenterWithModelBasedView(TestViewWithModel view)
                : base(view) { }
        }

        class TestPresenter : Presenter<IView>
        {
            public TestPresenter(IView view) : base(view) { }
        }
    }
}