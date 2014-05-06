// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using System.Web.UI;

    internal sealed class PageHost
    {
        readonly HttpPresenterBinder _presenterBinder;

        public PageHost(Page page, HttpContext context)
        {
            System.Diagnostics.Trace.TraceInformation("[HttpPresenterBinder] " + page.GetType().FullName);

            _presenterBinder = new HttpPresenterBinder(page.FindHosts(), context);

            page.InitComplete += (sender, e) => _presenterBinder.PerformBinding();
            page.Unload += (sender, e) => _presenterBinder.Release();
        }

        public static void RegisterControl<T>(T control, HttpContext context)
             where T : Control, IView
        {
            DebugCheck.NotNull(control);

            var page = control.Page;

            if (page == null) {
                throw new InvalidOperationException(
                    "Controls can only be registered once they have been added to the live control tree.");
            }

            var host = page.GetOrAddHost(context); 

            host.RegisterView(control);
        }

        public static void RegisterPage(Page page, HttpContext context)
        {
            DebugCheck.NotNull(page);

            page.GetOrAddHost(context);
        }

        public void RegisterView(IView view)
        {
            _presenterBinder.RegisterView(view);
        }
    }
}
