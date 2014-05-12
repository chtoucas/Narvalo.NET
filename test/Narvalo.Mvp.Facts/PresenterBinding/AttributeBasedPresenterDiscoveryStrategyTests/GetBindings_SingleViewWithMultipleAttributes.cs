﻿using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using NUnit.Framework;
using WebFormsMvp.Binder;

namespace WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests
{
    [TestFixture]
    public class GetBindings_SingleViewWithMultipleAttributes
    {
        [Test]
        public void AttributeBasedPresenterDiscoveryStrategy_GetBindings_SingleViewWithMultipleAttributes()
        {
            // Arrange
            var strategy = new AttributeBasedPresenterDiscoveryStrategy();
            var hosts = new object[0];
            var view1 = new View1();
            var views = new [] { view1 };

            // Act
            var results = strategy.GetBindings(hosts, views).ToArray();

            // Assert
            CollectionAssert.AreEqual(new[]
                {
                    new PresenterDiscoveryResult
                    (
                        new[] {view1},
                        @"AttributeBasedPresenterDiscoveryStrategy:
- found a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleViewWithMultipleAttributes+View1 (presenter type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleViewWithMultipleAttributes+Presenter1, view type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleViewWithMultipleAttributes+View1, binding mode: Default)
- found a [PresenterBinding] attribute on view instance WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleViewWithMultipleAttributes+View1 (presenter type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleViewWithMultipleAttributes+Presenter2, view type: WebFormsMvp.UnitTests.Binder.AttributeBasedPresenterDiscoveryStrategyTests.GetBindings_SingleViewWithMultipleAttributes+View1, binding mode: Default)",
                        new[]
                        {
                            new PresenterBinding(typeof(Presenter1), typeof(View1), BindingMode.Default, new[] {view1}), 
                            new PresenterBinding(typeof(Presenter2), typeof(View1), BindingMode.Default, new[] {view1}), 
                        }
                    )
                },
                results
            );
        }

        [PresenterBinding(typeof(Presenter2))]
        [PresenterBinding(typeof(Presenter1))]
        public class View1 : IView
        {
            public bool ThrowExceptionIfNoPresenterBound
            {
                get; set;
            }

            event EventHandler IView.Load
            {
                add { throw new NotImplementedException(); }
                remove { throw new NotImplementedException(); }
            }
        }

        public abstract class Presenter1 : IPresenter
        {
            public abstract HttpContextBase HttpContext { get; set; }
            public abstract HttpRequestBase Request { get; }
            public abstract HttpResponseBase Response { get; }
            public abstract HttpServerUtilityBase Server { get; }
            public abstract Cache Cache { get; }
            public abstract RouteData RouteData { get; }
            public abstract IAsyncTaskManager AsyncManager { get; set; }
            public abstract IMessageBus Messages { get; set; }
        }

        public abstract class Presenter2 : IPresenter
        {
            public abstract HttpContextBase HttpContext { get; set; }
            public abstract HttpRequestBase Request { get; }
            public abstract HttpResponseBase Response { get; }
            public abstract HttpServerUtilityBase Server { get; }
            public abstract Cache Cache { get; }
            public abstract RouteData RouteData { get; }
            public abstract void ReleaseView();
            public abstract IAsyncTaskManager AsyncManager { get; set; }
            public abstract IMessageBus Messages { get; set; }
        }
    }
}
