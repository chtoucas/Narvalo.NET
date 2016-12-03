// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using System.Web.Caching;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Properties;

    using static System.Diagnostics.Contracts.Contract;

    public abstract class HttpPresenter<TView, TViewModel>
        : Presenter<TView, TViewModel>, IHttpPresenter, Internal.IHttpPresenter
        where TView : class, IView<TViewModel>
        where TViewModel : class, new()
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
                Ensures(Result<IAsyncTaskManager>() != null);

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
                Ensures(Result<HttpContextBase>() != null);

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
                Ensures(Result<HttpApplicationStateBase>() != null);

                return HttpContext.Application;
            }
        }

        public Cache Cache => HttpContext.Cache;

        public HttpRequestBase Request
        {
            get
            {
                Ensures(Result<HttpRequestBase>() != null);

                return HttpContext.Request;
            }
        }

        public HttpResponseBase Response
        {
            get
            {
                Ensures(Result<HttpResponseBase>() != null);

                return HttpContext.Response;
            }
        }

        public HttpServerUtilityBase Server
        {
            get
            {
                Ensures(Result<HttpServerUtilityBase>() != null);

                return HttpContext.Server;
            }
        }

        public HttpSessionStateBase Session
        {
            get
            {
                Ensures(Result<HttpSessionStateBase>() != null);

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
