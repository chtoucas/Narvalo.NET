// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using System.Web.Caching;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Properties;

    public abstract class HttpPresenter<TView>
        : Presenter<TView>, IHttpPresenter, Internal.IHttpPresenter
        where TView : class, IView
    {
        private IAsyncTaskManager _asyncManager;
        private HttpContextBase _httpContext;

        protected HttpPresenter(TView view) : base(view)
        {
            Expect.NotNull(view);
        }

        public IAsyncTaskManager AsyncManager
        {
            get
            {
                Warrant.NotNull<IAsyncTaskManager>();

                if (_asyncManager == null)
                {
                    throw new InvalidOperationException(Strings.HttpPresenter_AsyncManagerPropertyIsNull);
                }

                return _asyncManager;
            }
            private set
            {
                Demand.NotNull(value);

                _asyncManager = value;
            }
        }

        public HttpContextBase HttpContext
        {
            get
            {
                Warrant.NotNull<HttpContextBase>();

                if (_httpContext == null)
                {
                    throw new InvalidOperationException(Strings.HttpPresenter_HttpContextIsNull);
                }

                return _httpContext;
            }
            private set
            {
                Demand.NotNull(value);

                _httpContext = value;
            }
        }

        public HttpApplicationStateBase Application
        {
            get
            {
                Warrant.NotNull<HttpApplicationStateBase>();

                return HttpContext.Application;
            }
        }

        public Cache Cache => HttpContext.Cache;

        public HttpRequestBase Request
        {
            get
            {
                Warrant.NotNull<HttpRequestBase>();

                return HttpContext.Request;
            }
        }

        public HttpResponseBase Response
        {
            get
            {
                Warrant.NotNull<HttpResponseBase>();

                return HttpContext.Response;
            }
        }

        public HttpServerUtilityBase Server
        {
            get
            {
                Warrant.NotNull<HttpServerUtilityBase>();

                return HttpContext.Server;
            }
        }

        public HttpSessionStateBase Session
        {
            get
            {
                Warrant.NotNull<HttpSessionStateBase>();

                return HttpContext.Session;
            }
        }

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
