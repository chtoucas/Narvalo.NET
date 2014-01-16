namespace Narvalo.Web.Optimization
{
    using System;
    using System.Web.Mvc;
    using System.Web.WebPages.Razor;
    using Narvalo.Web.Configuration;

    /// <remarks>
    /// WARNING: Malheureusement, en utilisant cette classe, on perd l'intellisense.
    /// </remarks>
    public class WhiteSpaceBusterMvcWebRazorHostFactory : MvcWebRazorHostFactory
    {
        static Lazy<bool> EnableWhiteSpaceBusting_ = new Lazy<bool>(() =>
        {
            return NarvaloWebConfigurationManager.GetOptimizationSection().EnableWhiteSpaceBusting;
        });

        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Narvalo.Web.Optimization.WhiteSpaceBusterMvcWebRazorHostFactory"/>.
        /// </summary>
        public WhiteSpaceBusterMvcWebRazorHostFactory() : base() { }

        protected static bool EnableWhiteSpaceBusting { get { return EnableWhiteSpaceBusting_.Value; } }

        public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
        {
            WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);

            if (host.IsSpecialPage || !EnableWhiteSpaceBusting) {
                return host;
            }

            return new WhiteSpaceBusterMvcWebPageRazorHost(
                virtualPath,
                physicalPath,
                new RazorOptimizer(WhiteSpaceBusterBuilder.Current.GetWhiteSpaceBuster()));
        }
    }
}
