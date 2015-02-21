// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using Narvalo;

    public sealed class LocalAssetProvider : AssetProviderBase
    {
        public LocalAssetProvider() { }

        public override void Initialize(string name, NameValueCollection config)
        {
            Require.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "LocalAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "Narvalo local asset provider.");
            }

            base.Initialize(name, config);
        }

        public override Uri GetFont(string relativePath)
        {
            return MakeUri_("~/asset/font/", relativePath);
        }

        public override Uri GetImage(string relativePath)
        {
            return MakeUri_("~/asset/img/", relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            return MakeUri_("~/asset/js/", relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            return MakeUri_("~/asset/css/", relativePath);
        }

        private static Uri MakeUri_(string basePath, string relativePath)
        {
            return new Uri(Combine_(basePath, relativePath), UriKind.Relative);
        }

        private static string Combine_(string basePath, string relativePath)
        {
            return VirtualPathUtility.ToAbsolute(
                relativePath.Length == 0 ? basePath : VirtualPathUtility.Combine(basePath, relativePath));
        }
    }
}
