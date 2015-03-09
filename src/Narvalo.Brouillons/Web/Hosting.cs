// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Configuration;

    public static class Hosting
    {
        ////static readonly AspNetHostingPermissionLevel[] TrustLevels_ = {
        ////    AspNetHostingPermissionLevel.Unrestricted,
        ////    AspNetHostingPermissionLevel.High,
        ////    AspNetHostingPermissionLevel.Medium,
        ////    AspNetHostingPermissionLevel.Low,
        ////    AspNetHostingPermissionLevel.Minimal
        ////};

        private static Lazy<bool> s_DeploymentIsRetail = new Lazy<bool>(DeploymentIsRetailThunk_);

        public static bool DeploymentIsRetail { get { return s_DeploymentIsRetail.Value; } }

        ///// <summary>
        ///// Récupération du niveau de confiance AspNet.
        ///// </summary>
        ///// <returns></returns>
        ////public static AspNetHostingPermissionLevel FindCurrentTrustLevel()
        ////{
        ////    foreach (var trustLevel in TrustLevels_) {
        ////        try {
        ////            new AspNetHostingPermission(trustLevel).Demand();
        ////        }
        ////        catch (SecurityException) {
        ////            continue;
        ////        }

        ////        return trustLevel;
        ////    }

        ////    return AspNetHostingPermissionLevel.None;
        ////}

        public static bool IsDebuggingEnabled(HttpContextBase httpContext)
        {
            return !DeploymentIsRetail && httpContext.IsDebuggingEnabled;
        }

        ////public static bool IsDebuggingEnabled(HttpContextBase httpContext, string virtualPath)
        ////{
        ////    if (IsDebuggingEnabled(httpContext)) {
        ////        return true;
        ////    }

        ////    var compilationSection
        ////        = (CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", virtualPath);

        ////    return compilationSection.Debug;
        ////}

        ////public static bool IsDebuggingEnabled()
        ////{
        ////    return IsDebuggingEnabled(new HttpContextWrapper(HttpContext.Current));
        ////}

        public static bool IsDebuggingEnabled(string virtualPath)
        {
            if (DeploymentIsRetail)
            {
                return false;
            }

            var compilationSection
                = (CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", virtualPath);

            return compilationSection.Debug;
        }

        private static bool DeploymentIsRetailThunk_()
        {
            var machineConfig = ConfigurationManager.OpenMachineConfiguration();
            var group = machineConfig.GetSectionGroup("system.web");

            if (group != null)
            {
                var deploymentSection = (DeploymentSection)group.Sections["deployment"];

                return deploymentSection.Retail;
            }

            return false;
        }
    }
}
