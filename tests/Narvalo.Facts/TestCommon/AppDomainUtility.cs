// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

// Adapted from aspnetwebstack\test\Microsoft.TestCommon\AppDomainUtils.cs
namespace Narvalo.TestCommon
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Web.Compilation;
    using System.Web.Hosting;

    internal static class AppDomainUtility
    {
        // Allow a test to modify static fields in an independent appdomain so that
        // other tests will not be affected.
        public static void RunInSeparateAppDomain(Action action)
        {
            RunInSeparateAppDomain(new AppDomainSetup(), action);
        }

        public static void RunInSeparateAppDomain(AppDomainSetup setup, Action action)
        {
            var dir = Path.GetDirectoryName(typeof(AppDomainUtility).Assembly.CodeBase).Replace("file:\\", String.Empty);
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
                AppDomainHelper helper = appDomain.CreateInstanceAndUnwrap(typeof(AppDomainUtility).Assembly.FullName, typeof(AppDomainHelper).FullName) as AppDomainHelper;
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

        public class AppDomainHelper : MarshalByRefObject
        {
            [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            public void Run(Action action)
            {
                action();
            }
        }
    }
}
