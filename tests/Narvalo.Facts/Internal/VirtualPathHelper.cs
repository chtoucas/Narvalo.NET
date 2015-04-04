// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.Hosting;

    internal static class VirtualPathHelper
    {
        private static readonly object s_Lock = new Object();
        private static volatile bool s_Initialized = false;

        public static void InitializeFakeContext()
        {
            if (!s_Initialized)
            {
                lock (s_Lock)
                {
                    if (!s_Initialized)
                    {
                        Initialize_();
                        s_Initialized = true;
                    }
                }
            }
        }

        // Adapted from http://stackoverflow.com/questions/655134/unit-testing-code-that-calls-virtualpathutility-toabsolute
        private static void Initialize_()
        {
            InitializeAppDomain_("/", AppDomain.CurrentDomain.BaseDirectory);

            using (var sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                var worker = new SimpleWorkerRequest("default.aspx", String.Empty, sw);

                HttpContext.Current = new HttpContext(worker);
            }
        }

        private static void InitializeAppDomain_(string appVirtualDir, string appPhysicalDir)
        {
            AppDomain.CurrentDomain.SetData(".appDomain", "*");
            AppDomain.CurrentDomain.SetData(".appPath", appPhysicalDir);
            AppDomain.CurrentDomain.SetData(".appVPath", appVirtualDir);
            AppDomain.CurrentDomain.SetData(".hostingVirtualPath", appVirtualDir);
            AppDomain.CurrentDomain.SetData(".hostingInstallDir", HttpRuntime.AspInstallDirectory);
        }
    }
}
