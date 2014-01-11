namespace Narvalo.Web.Internal
{
    using System.Configuration;
    using System.Web;
    using System.Web.Configuration;

    class HttpApplicationUtility
    {
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
