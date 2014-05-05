// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Web;
    using System.Web.Caching;

    public abstract class HttpPresenter<TView> : IHttpPresenter<TView>
        where TView : class, IView
    {
        readonly TView _view;

        protected HttpPresenter(TView view)
        {
            Require.NotNull(view, "view");

            _view = view;
        }

        public IMessageBus Messages { get; set; }

        public TView View { get { return _view; } }

        public HttpContextBase HttpContext { get; set; }

        public HttpApplicationStateBase Application { get { return HttpContext.Application; } }

        public Cache Cache { get { return HttpContext.Cache; } }

        public HttpRequestBase Request { get { return HttpContext.Request; } }

        public HttpResponseBase Response { get { return HttpContext.Response; } }

        public HttpServerUtilityBase Server { get { return HttpContext.Server; } }

        public HttpSessionStateBase Session { get { return HttpContext.Session; } }
    }
}
