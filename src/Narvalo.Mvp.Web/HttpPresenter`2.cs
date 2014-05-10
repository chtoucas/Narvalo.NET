// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Web;
    using System.Web.Caching;
    using Narvalo.Mvp;

    public abstract class HttpPresenter<TView, TViewModel>
        : Presenter<TView, TViewModel>, IHttpPresenter<TView>
        where TView : class, IView<TViewModel>
        where TViewModel : class, new()
    {
        protected HttpPresenter(TView view) : base(view) { }

        public HttpContextBase HttpContext { get; set; }

        public IMessageCoordinator Messages { get; set; }

        public HttpApplicationStateBase Application { get { return HttpContext.Application; } }

        public Cache Cache { get { return HttpContext.Cache; } }

        public HttpRequestBase Request { get { return HttpContext.Request; } }

        public HttpResponseBase Response { get { return HttpContext.Response; } }

        public HttpServerUtilityBase Server { get { return HttpContext.Server; } }

        public HttpSessionStateBase Session { get { return HttpContext.Session; } }
    }
}
