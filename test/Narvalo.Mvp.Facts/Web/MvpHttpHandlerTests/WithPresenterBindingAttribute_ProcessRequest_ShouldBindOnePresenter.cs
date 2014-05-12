﻿using System.Web;
using NUnit.Framework;
using WebFormsMvp.Web;

namespace WebFormsMvp.UnitTests.Web.MvpHttpHandlerTests
{
    [TestFixture]
    public class WithPresenterBindingAttribute_ShouldBindOnePresenter
    {
        [Test]
        public void MvpHttpHandler_WithPresenterBindingAttribute_ProcessRequest_ShouldBindOnePresenter()
        {
            // Arrange
            var httpContext = new HttpContext(new HttpRequest("c:\test.txt", "http://test", "a=b"), new HttpResponse(null));
            var handler = new TestHandler();

            // Act
            handler.ProcessRequest(httpContext);

            // Assert
            Assert.AreEqual(1, handler.PresentersBound);
        }

        public interface ITestView : IView
        {
            int PresentersBound { get; set; }
        }

        [PresenterBinding(typeof(TestPresenter))]
        class TestHandler : MvpHttpHandler, ITestView
        {
            public int PresentersBound { get; set; }
        }

        public class TestPresenter : Presenter<ITestView>
        {
            public TestPresenter(ITestView view)
                : base(view)
            {
                view.PresentersBound++;
            }
        }
    }
}