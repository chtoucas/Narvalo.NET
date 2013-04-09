namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Collections.Specialized;

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
                config.Add("description", "ASP.NET default asset path provider.");
            }

            base.Initialize(name, config);
        }

        public override AssetFile GetImage(string relativePath)
        {
            return new AssetFile("/Content/" + relativePath);
        }

        public override AssetFile GetScript(string relativePath)
        {
            return new AssetFile("/Scripts/" + relativePath);
        }

        public override AssetFile GetStyle(string relativePath)
        {
            return new AssetFile("/Styles/" + relativePath);
        }
    }
}
