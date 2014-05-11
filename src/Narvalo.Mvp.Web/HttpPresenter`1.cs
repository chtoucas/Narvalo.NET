// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Web;
    using System.Web.Caching;
    using Narvalo.Mvp;

    public abstract class HttpPresenter<TView> 
        : Presenter<TView>, IHttpPresenter, Internal.IHttpPresenter
        where TView : class, IView
    {
        protected HttpPresenter(TView view) : base(view) { }

        public IAsyncTaskManager AsyncManager { get; private set; }

        public HttpContextBase HttpContext { get; private set; }

        public IMessageCoordinator Messages { get; private set; }

        public HttpApplicationStateBase Application { get { return HttpContext.Application; } }

        public Cache Cache { get { return HttpContext.Cache; } }

        public HttpRequestBase Request { get { return HttpContext.Request; } }

        public HttpResponseBase Response { get { return HttpContext.Response; } }

        public HttpServerUtilityBase Server { get { return HttpContext.Server; } }

        public HttpSessionStateBase Session { get { return HttpContext.Session; } }

        IAsyncTaskManager Internal.IHttpPresenter.AsyncManager
        {
            set { AsyncManager = value; }
        }

        HttpContextBase Internal.IHttpPresenter.HttpContext
        {
            set { HttpContext = value; }
        }

        IMessageCoordinator Internal.IHttpPresenter.Messages
        {
            set { Messages = value; }
        }
    }
}
