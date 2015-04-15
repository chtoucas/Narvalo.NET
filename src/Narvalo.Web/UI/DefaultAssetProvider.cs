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
    public sealed class DefaultAssetProvider : AssetProvider
    {
        internal const string CustomDefaultName = "DefaultAssetProvider";

        // WARNING: If you change the name of this type, be sure to also update
        // AssetSection.InternalDefaultProviderPropertyValue.
        public DefaultAssetProvider()
        {
            DefaultName = CustomDefaultName;
            DefaultDescription = Strings.DefaultAssetProvider_Description;
        }

        public override Uri GetFontUri(string relativePath)
        {
            return MakeUri_("~/fonts/", relativePath);
        }

        public override Uri GetImageUri(string relativePath)
        {
            return MakeUri_("~/Images/", relativePath);
        }

        public override Uri GetScriptUri(string relativePath)
        {
            return MakeUri_("~/Scripts/", relativePath);
        }

        public override Uri GetStyleUri(string relativePath)
        {
            return MakeUri_("~/Content/", relativePath);
        }

        private static Uri MakeUri_(string baseIntermediatePath, string relativePath)
        {
            Require.NotNull(relativePath, "relativePath");
            Contract.Requires(baseIntermediatePath != null);
            Contract.Requires(baseIntermediatePath.Length != 0);
            Contract.Ensures(Contract.Result<Uri>() != null);

            // NB: If basePath or relativePath is null or empty, VirtualPathUtility.Combine will throw,
            // which is of course exactly what we want.
            var uriString = VirtualPathUtility.ToAbsolute(
                relativePath.Length == 0 ? baseIntermediatePath : VirtualPathUtility.Combine(baseIntermediatePath, relativePath));

            return new Uri(uriString, UriKind.Relative);
        }
    }
}
