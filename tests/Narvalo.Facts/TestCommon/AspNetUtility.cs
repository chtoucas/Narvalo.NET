// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.TestCommon
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.WebPages;
    using System.Web.WebPages.TestUtils;

    // FIXME: Reset environment after.
    public static class AspNetUtility
    {
        public static IDisposable CreateHttpContext()
        {
            return CreateHttpContext("default.aspx", "http://tempuri.org/", String.Empty);
        }

        public static IDisposable CreateHttpContext(string page, string query)
        {
            using (var sw = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture))
            {
                var worker = new SimpleWorkerRequest(page, query, sw);
                var httpContext = new HttpContext(worker);
                HttpContext.Current = httpContext;

                return new DisposableAction(RestoreHttpContext);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public static IDisposable CreateHttpContext(string fileName, string url, string query)
        {
            using (var sw = new StringWriter(new StringBuilder(), CultureInfo.InvariantCulture))
            {
                var request = new HttpRequest(fileName, url, query);
                var httpContext = new HttpContext(request, new HttpResponse(sw));
                HttpContext.Current = httpContext;

                return new DisposableAction(RestoreHttpContext);
            }
        }

        public static void RestoreHttpContext()
        {
            HttpContext.Current = null;
        }

        public static void SetupAspNetDomain()
        {
            SetupAspNetDomain("/", AppDomain.CurrentDomain.BaseDirectory);
        }

        // Adapted from http://stackoverflow.com/questions/655134/unit-testing-code-that-calls-virtualpathutility-toabsolute
        public static void SetupAspNetDomain(string appVirtualDirectory, string appPhysicalDirectory)
        {
            var appDomain = AppDomain.CurrentDomain;

            appDomain.SetData(".appDomain", "*");
            appDomain.SetData(".appPath", appPhysicalDirectory);
            appDomain.SetData(".appVPath", appVirtualDirectory);
            appDomain.SetData(".hostingVirtualPath", appVirtualDirectory);
            appDomain.SetData(".hostingInstallDir", HttpRuntime.AspInstallDirectory);
        }
    }
}
