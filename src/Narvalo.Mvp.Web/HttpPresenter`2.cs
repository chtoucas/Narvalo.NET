// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Caching;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Properties;

    public abstract class HttpPresenter<TView, TViewModel>
        : Presenter<TView, TViewModel>, IHttpPresenter, Internal.IHttpPresenter
        where TView : class, IView<TViewModel>
        where TViewModel : class, new()
    {
        private IAsyncTaskManager _asyncManager;
        private HttpContextBase _httpContext;

        protected HttpPresenter(TView view) : base(view) { }

        public IAsyncTaskManager AsyncManager
        {
            get
            {
                if (_asyncManager == null)
                {
                    throw new InvalidOperationException(Strings.HttpPresenter_AsyncManagerPropertyIsNull);
                }

                return _asyncManager;
            }
            private set
            {
                Debug.Assert(value != null);

                _asyncManager = value;
            }
        }

        public HttpContextBase HttpContext
        {
            get
            {
                if (_httpContext == null)
                {
                    throw new InvalidOperationException(Strings.HttpPresenter_HttpContextIsNull);
                }

                return _httpContext;
            }
            private set
            {
                Debug.Assert(value != null);

                _httpContext = value;
            }
        }

        public HttpApplicationStateBase Application => HttpContext.Application;

        public Cache Cache => HttpContext.Cache;

        public HttpRequestBase Request => HttpContext.Request;

        public HttpResponseBase Response => HttpContext.Response;

        public HttpServerUtilityBase Server => HttpContext.Server;

        public HttpSessionStateBase Session => HttpContext.Session;

        IAsyncTaskManager Internal.IHttpPresenter.AsyncManager
        {
            set { AsyncManager = value; }
        }

        HttpContextBase Internal.IHttpPresenter.HttpContext
        {
            set { HttpContext = value; }
        }
    }
}
