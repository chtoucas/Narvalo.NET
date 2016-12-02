// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif
    using System.Web;
    using System.Web.UI;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Internal;

    using static System.Diagnostics.Contracts.Contract;

    public sealed class PageHost
    {
        private static readonly string s_PageHostKey = typeof(PageHost).FullName;

        private readonly HttpPresenterBinder _presenterBinder;

        public PageHost(Page page, HttpContext context)
        {
            Require.NotNull(page, nameof(page));
            Expect.NotNull(context);

            var hosts = FindHosts(page);

            _presenterBinder = HttpPresenterBinderFactory.Create(hosts, context);

            _presenterBinder.PresenterCreated += (sender, e) =>
            {
                var presenter = e.Presenter as Internal.IHttpPresenter;
                if (presenter != null)
                {
                    presenter.AsyncManager = new PageAsyncTaskManager(page);
                }
            };

            // On page's initialization, bind the presenter.
            page.InitComplete += (sender, e) => _presenterBinder.PerformBinding();

            // Ensures that any attempt to use the message bus fails after pre-rendering completes.
            page.PreRenderComplete += (sender, e) => _presenterBinder.MessageCoordinator.Close();

            // On unloading the page, release the binder.
            page.Unload += (sender, e) => _presenterBinder.Release();
        }

        public static PageHost Register(Page page, HttpContext context)
        {
            Require.NotNull(page, nameof(page));
            Expect.NotNull(context);
            Ensures(Result<PageHost>() != null);

            var pageContext = page.Items;

            if (pageContext.Contains(s_PageHostKey))
            {
                var host = pageContext[s_PageHostKey];
                Assume(host != null, "At this point, we are sure that this variable is not null.");
                return (PageHost)host;
            }
            else
            {
                var host = new PageHost(page, context);
                pageContext[s_PageHostKey] = host;

                return host;
            }
        }

        public void RegisterView(IView view)
        {
            Expect.NotNull(view);

            _presenterBinder.RegisterView(view);
        }

        private static IEnumerable<Control> FindHosts(Page page)
        {
            Ensures(Result<IEnumerable<Control>>() != null);

            yield return page;

            var masterHost = page.Master;

            while (masterHost != null)
            {
                yield return masterHost;

                masterHost = masterHost.Master;
            }
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Invariant(_presenterBinder != null);
        }

#endif
    }
}
