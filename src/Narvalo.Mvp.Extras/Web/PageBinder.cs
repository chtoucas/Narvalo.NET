// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using Narvalo.Mvp.PresenterBinding;

    internal sealed class PageBinder
    {
        readonly PresenterBinder _presenterBinder;

        public PageBinder(Page page, HttpContext httpContext)
        {
            DebugCheck.NotNull(page);
            DebugCheck.NotNull(httpContext);

            var hosts = FindHosts_(page);

            _presenterBinder = new PresenterBinder(hosts);
            _presenterBinder.PresenterCreated += (sender, e) =>
            {
                var presenter = e.Presenter as IHttpPresenter;
                if (presenter != null) {
                    presenter.HttpContext = new HttpContextWrapper(httpContext);
                }
            };

            page.InitComplete += (sender, e) => _presenterBinder.PerformBinding();
            page.Unload += (sender, e) => _presenterBinder.Release();
        }

        public void RegisterView(IView view)
        {
            DebugCheck.NotNull(view);

            _presenterBinder.RegisterView(view);
        }

        static IEnumerable<object> FindHosts_(Page page)
        {
            yield return page;

            var masterHost = page.Master;

            while (masterHost != null) {
                yield return masterHost;

                masterHost = masterHost.Master;
            }
        }
    }
}
