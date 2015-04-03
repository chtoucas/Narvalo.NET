// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web;

    using Narvalo.Web.Properties;

    /// <summary>
    /// Provides a default implementation for the asset provider model.
    /// </summary>
    public sealed class DefaultAssetProvider : AssetProviderBase
    {
        public DefaultAssetProvider()
        {
            DefaultName = "DefaultAssetProvider";
            DefaultDescription = Strings_Web.DefaultAssetProvider_Description;
        }

        public override Uri GetFont(string relativePath)
        {
            return MakeUri_("~/fonts/", relativePath);
        }

        public override Uri GetImage(string relativePath)
        {
            return MakeUri_("~/Images/", relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            return MakeUri_("~/Scripts/", relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            return MakeUri_("~/Content/", relativePath);
        }

        private static Uri MakeUri_(string basePath, string relativePath)
        {
            Contract.Requires(basePath != null);
            Contract.Requires(basePath.Length != 0);
            Contract.Requires(relativePath != null);
            Contract.Requires(relativePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            // NB: If basePath or relativePath is null or empty, VirtualPathUtility.Combine will throw,
            // which is of course exactly what we want.
            // REVIEW: Cf. http://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
            var uriString = VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(basePath, relativePath));

            return new Uri(uriString, UriKind.Relative);
        }
    }
}
