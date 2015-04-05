// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

// Borrowed from aspnetwebstack\test\Microsoft.TestCommon\AppDomainUtils.cs
namespace System.Web.WebPages.TestUtils
{
    using System.IO;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Hosting;

    internal static class AppDomainUtils
    {
        // Allow a test to modify static fields in an independent appdomain so that
        // other tests will not be affected.
        public static void RunInSeparateAppDomain(Action action)
        {
            RunInSeparateAppDomain(new AppDomainSetup(), action);
        }

        public static void RunInSeparateAppDomain(AppDomainSetup setup, Action action)
        {
            var dir = Path.GetDirectoryName(typeof(AppDomainUtils).Assembly.CodeBase).Replace("file:\\", String.Empty);
            setup.PrivateBinPath = dir;
            setup.ApplicationBase = dir;
            setup.ApplicationName = Guid.NewGuid().ToString();
            setup.ShadowCopyFiles = "true";
            setup.ShadowCopyDirectories = setup.ApplicationBase;
            setup.CachePath = Path.Combine(Path.GetTempPath(), setup.ApplicationName);

            AppDomain appDomain = null;
            try
            {
                appDomain = AppDomain.CreateDomain(setup.ApplicationName, null, setup);
                AppDomainHelper helper = appDomain.CreateInstanceAndUnwrap(typeof(AppDomainUtils).Assembly.FullName, typeof(AppDomainHelper).FullName) as AppDomainHelper;
                helper.Run(action);
            }
            finally
            {
                if (appDomain != null)
                {
                    AppDomain.Unload(appDomain);
                }
            }
        }

        ////public static void SetPreAppStartStage()
        ////{
        ////    var stage = typeof(BuildManager).GetProperty("PreStartInitStage", BindingFlags.Static | BindingFlags.NonPublic);
        ////    var value = ((FieldInfo)typeof(BuildManager).Assembly.GetType("System.Web.Compilation.PreStartInitStage").GetMember("DuringPreStartInit")[0]).GetValue(null);
        ////    stage.SetValue(null, value, new object[] { });
        ////    SetAppData();
        ////    var env = new HostingEnvironment();
        ////}

        ////public static void SetAppData()
        ////{
        ////    var appdomain = AppDomain.CurrentDomain;

        ////    // Set some dummy values to make the appdomain seem more like a ASP.NET hosted one
        ////    appdomain.SetData(".appDomain", "*");
        ////    appdomain.SetData(".appId", "appId");
        ////    appdomain.SetData(".appPath", "appPath");
        ////    appdomain.SetData(".appVPath", "/WebSite1");
        ////    appdomain.SetData(".domainId", "1");
        ////}

        public class AppDomainHelper : MarshalByRefObject
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            public void Run(Action action)
            {
                action();
            }
        }
    }
}
