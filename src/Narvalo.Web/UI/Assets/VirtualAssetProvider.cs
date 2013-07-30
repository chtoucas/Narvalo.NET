namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Collections.Specialized;
    using System.Web;

    public class VirtualAssetProvider : AssetProviderBase
    {
        public VirtualAssetProvider() : base() { }

        public override void Initialize(string name, NameValueCollection config)
        {
            Requires.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "DefaultAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "ASP.NET default asset path provider.");
            }

            base.Initialize(name, config);
        }

        public override VirtualAssetFile GetImage(string relativePath)
        {
            return new VirtualAssetFile("/assets/img/" + relativePath);
        }

        public override VirtualAssetFile GetScript(string relativePath)
        {
            return new VirtualAssetFile("/assets/js/" + relativePath);
        }

        public override VirtualAssetFile GetStyle(string relativePath)
        {
            return new VirtualAssetFile("/assets/css/" + relativePath);
        }
    }
}
