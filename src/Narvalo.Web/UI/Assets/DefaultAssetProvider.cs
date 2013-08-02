namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Web;

    public class DefaultAssetProvider : AssetProviderBase
    {
        public DefaultAssetProvider() : base() { }

        public override void Initialize(string name, NameValueCollection config)
        {
            Requires.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "DefaultAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "ASP.NET default asset provider.");
            }

            base.Initialize(name, config);

            // FIXME: On vérifie qu'il n'y a pas de champs inconnu restant.
            config.Remove("description");

            if (config.Count > 0) {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr)) {
                    throw new ProviderException("Unrecognized attribute: " + attr);
                }
            }
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

        static Uri MakeUri_(string basePath, string relativePath)
        {
            return new Uri(Combine_(basePath, relativePath), UriKind.Relative);
        }

        static string Combine_(string basePath, string relativePath)
        {
            return VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(basePath, relativePath));
        }
    }
}
