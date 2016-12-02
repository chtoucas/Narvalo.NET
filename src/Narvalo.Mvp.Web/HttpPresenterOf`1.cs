// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Caching;

    using Narvalo.Mvp;

    using static System.Diagnostics.Contracts.Contract;

    public abstract class HttpPresenterOf<TViewModel>
        : PresenterOf<TViewModel>, IHttpPresenter, Internal.IHttpPresenter
        where TViewModel : class, new()
    {
        private IAsyncTaskManager _asyncManager;
        private HttpContextBase _httpContext;

        protected HttpPresenterOf(IView<TViewModel> view) : base(view)
        {
            Expect.NotNull(view);
        }

        [ContractVerification(false)]  // Cf. HttpPresenter<TView>
        public IAsyncTaskManager AsyncManager
        {
            get
            {
                Ensures(Result<IAsyncTaskManager>() != null);

                return _asyncManager;
            }
            private set
            {
                Demand.NotNull(value);

                _asyncManager = value;
            }
        }

        [ContractVerification(false)]  // Cf. HttpPresenter<TView>
        public HttpContextBase HttpContext
        {
            get
            {
                Ensures(Result<HttpContextBase>() != null);

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

        public Cache Cache { get { return HttpContext.Cache; } }

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
