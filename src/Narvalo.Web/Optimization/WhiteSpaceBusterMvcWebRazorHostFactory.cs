// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using System.Web.WebPages.Razor;

    using Narvalo.Web.Configuration;

    /// <remarks>
    /// Malheureusement, en utilisant cette classe, on perd l'intellisense.
    /// </remarks>
    public sealed class WhiteSpaceBusterMvcWebRazorHostFactory : MvcWebRazorHostFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Narvalo.Web.Optimization.WhiteSpaceBusterMvcWebRazorHostFactory"/> class.
        /// </summary>
        public WhiteSpaceBusterMvcWebRazorHostFactory() { }

        public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
        {
            WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);

            if (host.IsSpecialPage)
            {
                return host;
            }

            var section = NarvaloWebConfigurationManager.OptimizationSection;
            if (!section.EnableWhiteSpaceBusting)
            {
                return host;
            }

            return new WhiteSpaceBusterMvcWebPageRazorHost(
                virtualPath,
                physicalPath,
                new RazorOptimizer(WhiteSpaceBusterProvider.Current.Buster));
        }
    }
}
