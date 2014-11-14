// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Mvp
{
    using System.Web;
    using System.Web.Caching;
    using Narvalo.Mvp;

    public abstract class HttpPresenter<TView> 
        : Presenter<TView>, IHttpPresenter, Narvalo.Web.Internal.IHttpPresenter
        where TView : class, IView
    {
        protected HttpPresenter(TView view) : base(view) { }

        public IAsyncTaskManager AsyncManager { get; private set; }

        public HttpContextBase HttpContext { get; private set; }

        public HttpApplicationStateBase Application { get { return HttpContext.Application; } }

        public Cache Cache { get { return HttpContext.Cache; } }

        public HttpRequestBase Request { get { return HttpContext.Request; } }

        public HttpResponseBase Response { get { return HttpContext.Response; } }

        public HttpServerUtilityBase Server { get { return HttpContext.Server; } }

        public HttpSessionStateBase Session { get { return HttpContext.Session; } }

        IAsyncTaskManager Narvalo.Web.Internal.IHttpPresenter.AsyncManager
        {
            set { AsyncManager = value; }
        }

        HttpContextBase Narvalo.Web.Internal.IHttpPresenter.HttpContext
        {
            set { HttpContext = value; }
        }
    }
}
