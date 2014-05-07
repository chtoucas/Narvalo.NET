// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Internal
{
    using System.Collections.Generic;
    using System.Web;
    using Narvalo.Mvp.PresenterBinding;

    internal sealed class HttpPresenterBinder
    {
        readonly PresenterBinder _presenterBinder;

        public HttpPresenterBinder(object host, HttpContext context)
            : this(new[] { host }, context) { }

        public HttpPresenterBinder(IEnumerable<object> hosts, HttpContext context)
        {
            DebugCheck.NotNull(hosts);
            DebugCheck.NotNull(context);

            _presenterBinder = new PresenterBinder(hosts);
            _presenterBinder.PresenterCreated += (sender, e) =>
            {
                var presenter = e.Presenter as IHttpPresenter;
                if (presenter != null) {
                    presenter.HttpContext = new HttpContextWrapper(context);
                }
            };
        }

        public void PerformBinding()
        {
            _presenterBinder.PerformBinding();
        }

        public void RegisterView(IView view)
        {
            _presenterBinder.RegisterView(view);
        }

        public void Release()
        {
            _presenterBinder.Release();
        }
    }
}
