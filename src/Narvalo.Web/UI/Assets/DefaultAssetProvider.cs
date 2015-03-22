// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Web;

    public sealed class DefaultAssetProvider : AssetProviderBase
    {
        public DefaultAssetProvider() { }

        public override void Initialize(string name, NameValueCollection config)
        {
            Require.NotNull(config, "config");

            if (String.IsNullOrEmpty(name))
            {
                name = "DefaultAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "ASP.NET default asset provider.");
            }

            base.Initialize(name, config);

            // FIXME: Vérifier qu'il n'y a pas de champs inconnu restant.
            config.Remove("description");

            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                {
                    throw new ProviderException("Unrecognized attribute: " + attr);
                }
            }
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
            return new Uri(Combine_(basePath, relativePath), UriKind.Relative);
        }

        // REVIEW: Cf. http://stackoverflow.com/questions/1268738/asp-net-mvc-find-absolute-path-to-the-app-data-folder-from-controller
        private static string Combine_(string basePath, string relativePath)
        {
            return VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(basePath, relativePath));
        }
    }
}
