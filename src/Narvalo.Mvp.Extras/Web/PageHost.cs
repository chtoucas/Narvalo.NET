// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using System.Web.UI;

    internal static class PageHost
    {
        readonly static string CacheKey_ = typeof(PageHost).FullName;

        public static void Register<T>(T control, HttpContext httpContext)
            where T : Control, IView
        {
            Require.NotNull(control, "control");
            Require.NotNull(httpContext, "httpContext");

            var page = control.Page;

            if (page == null) {
                throw new InvalidOperationException(
                    "Controls can only be registered once they have been added to the live control tree. The best place to register them is within the control's Init event.");
            }

            var host = GetOrAddBinder_(page, httpContext);
            host.RegisterView(control);
        }

        static PageBinder GetOrAddBinder_(Page page, HttpContext httpContext)
        {
            DebugCheck.NotNull(page);

            var pageContext = page.Items;

            if (pageContext.Contains(CacheKey_)) {
                return (PageBinder)pageContext[CacheKey_];
            }

            var binder = new PageBinder(page, httpContext);

            pageContext[CacheKey_] = binder;

            return binder;
        }
    }
}
