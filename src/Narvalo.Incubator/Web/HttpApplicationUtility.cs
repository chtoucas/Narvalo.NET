namespace Narvalo.Web
{
    using System.Configuration;
    //using System.Security;
    using System.Web;
    using System.Web.Configuration;

    public class HttpApplicationUtility
    {
        //static readonly AspNetHostingPermissionLevel[] TrustLevels_ = {
        //    AspNetHostingPermissionLevel.Unrestricted,
        //    AspNetHostingPermissionLevel.High,
        //    AspNetHostingPermissionLevel.Medium,
        //    AspNetHostingPermissionLevel.Low,
        //    AspNetHostingPermissionLevel.Minimal
        //};

        ///// <summary>
        ///// Récupération du niveau de confiance AspNet.
        ///// </summary>
        ///// <returns></returns>
        //public static AspNetHostingPermissionLevel FindCurrentTrustLevel()
        //{
        //    foreach (var trustLevel in TrustLevels_) {
        //        try {
        //            new AspNetHostingPermission(trustLevel).Demand();
        //        }
        //        catch (SecurityException) {
        //            continue;
        //        }

        //        return trustLevel;
        //    }

        //    return AspNetHostingPermissionLevel.None;
        //}

        public static bool IsDebuggingEnabled(HttpContextBase httpContext)
        {
            return httpContext.IsDebuggingEnabled && !DeploymentIsRetail();
        }

        //public static bool IsDebuggingEnabled(HttpContextBase httpContext, string virtualPath)
        //{
        //    if (IsDebuggingEnabled(httpContext)) {
        //        return true;
        //    }

        //    var compilationSection
        //        = (CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", virtualPath);

        //    return compilationSection.Debug;
        //}

        //public static bool IsDebuggingEnabled()
        //{
        //    return IsDebuggingEnabled(new HttpContextWrapper(HttpContext.Current));
        //}

        public static bool IsDebuggingEnabled(string virtualPath)
        {
            if (DeploymentIsRetail()) {
                return false;
            }

            var compilationSection 
                = (CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", virtualPath);

            return compilationSection.Debug;
        }

        public static bool DeploymentIsRetail()
        {
            var machineConfig = ConfigurationManager.OpenMachineConfiguration();
            var group = machineConfig.GetSectionGroup("system.web");

            if (group != null) {
                var deploymentSection = (DeploymentSection)group.Sections["deployment"];

                return deploymentSection.Retail;
            }

            return false;
        }
    }
}
