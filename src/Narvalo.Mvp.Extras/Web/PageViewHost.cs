// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class PageViewHost
    {
        readonly static string viewHostCacheKey = typeof(PageViewHost).FullName + ".PageContextKey";

        readonly PresenterBinder presenterBinder;

        public PageViewHost(Page page, HttpContext httpContext)
        {
            Require.NotNull(page, "page");
            Require.NotNull(httpContext, "httpContext");

            var hosts = FindHosts(page).ToArray();

            presenterBinder = new AspNetPresenterBinder(hosts, httpContext);

            page.InitComplete += Page_InitComplete;
            page.PreRenderComplete += Page_PreRenderComplete;
            page.Unload += Page_Unload;
        }

        public static void Register<T>(T control, HttpContext httpContext, bool enableAutomaticDataBinding)
            where T : Control, IView
        {
            Require.NotNull(control, "control");
            Require.NotNull(httpContext, "httpContext");

            if (control.Page == null) {
                throw new InvalidOperationException("Controls can only be registered once they have been added to the live control tree. The best place to register them is within the control's Init event.");
            }

            var viewHost = FindViewHost(control, httpContext);
            viewHost.RegisterView(control);

            // This event is raised after any async page tasks have completed, so it is safe to data-bind
            if (enableAutomaticDataBinding) {
                control.Page.PreRenderComplete += (sender, e) => control.DataBind();
            }
        }

        internal static IEnumerable<object> FindHosts(Page page)
        {
            yield return page;

            var masterHost = page.Master;

            while (masterHost != null) {
                yield return masterHost;

                masterHost = masterHost.Master;
            }
        }

        internal static PageViewHost FindViewHost(Control control, HttpContext httpContext)
        {
            var pageContext = control.Page.Items;

            if (pageContext.Contains(viewHostCacheKey))
                return (PageViewHost)pageContext[viewHostCacheKey];

            var host = new PageViewHost(control.Page, httpContext);

            pageContext[viewHostCacheKey] = host;

            return host;
        }

        void RegisterView(IView view)
        {
            presenterBinder.RegisterView(view);
        }

        void Page_InitComplete(object sender, EventArgs e)
        {
            presenterBinder.PerformBinding();
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            //presenterBinder.Messages.Close();
        }

        void Page_Unload(object sender, EventArgs e)
        {
            presenterBinder.Release();
        }
    }
}
