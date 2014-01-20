namespace Narvalo.Web.Optimization
{
    using System.Web.Mvc;
    using System.Web.WebPages.Razor;
    using Narvalo.Web.Configuration;

    /// <remarks>
    /// Malheureusement, en utilisant cette classe, on perd l'intellisense.
    /// </remarks>
    public class WhiteSpaceBusterMvcWebRazorHostFactory : MvcWebRazorHostFactory
    {
        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Narvalo.Web.Optimization.WhiteSpaceBusterMvcWebRazorHostFactory"/>.
        /// </summary>
        public WhiteSpaceBusterMvcWebRazorHostFactory() : base() { }

        protected static bool EnableWhiteSpaceBusting
        {
            get { return NarvaloWebConfigurationManager.OptimizationSection.EnableWhiteSpaceBusting; }
        }

        public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
        {
            WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);

            if (host.IsSpecialPage || !EnableWhiteSpaceBusting) {
                return host;
            }

            return new WhiteSpaceBusterMvcWebPageRazorHost(
                virtualPath,
                physicalPath,
                new RazorOptimizer(WhiteSpaceBusterProvider.Current.WhiteSpaceBuster));
        }
    }
}
