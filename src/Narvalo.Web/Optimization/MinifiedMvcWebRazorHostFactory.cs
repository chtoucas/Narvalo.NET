namespace Narvalo.Web.Optimization
{
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.WebPages.Razor;

    public class MinifiedMvcWebRazorHostFactory : MvcWebRazorHostFactory
    {
        public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
        {
            WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);

            if (IsDebuggingEnabled_(virtualPath) || host.IsSpecialPage) {
                return host;
            }

            return new MinifiedMvcWebPageRazorHost(virtualPath, physicalPath);
        }

        bool IsDebuggingEnabled_(string virtualPath)
        {
            return ((CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", virtualPath)).Debug;
        }

        //bool IsDebuggingEnabled_() {
        //    if (HttpContext.Current != null) {
        //        return HttpContext.Current.IsDebuggingEnabled;
        //    }

        //    string virtualPath = ((WebPageRazorHost)Host).VirtualPath;
        //    return ((CompilationSection)WebConfigurationManager.GetSection("system.web/compilation", virtualPath)).Debug;
        //}
    }
}
