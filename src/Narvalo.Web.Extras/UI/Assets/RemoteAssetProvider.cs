namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using Narvalo;
    using Narvalo.Collections;

    public class RemoteAssetProvider : AssetProviderBase
    {
        Uri _baseUri;

        public RemoteAssetProvider() : base() { }

        public override void Initialize(string name, NameValueCollection config)
        {
            Requires.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "RemoteAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "Narvalo remote asset provider.");
            }

            base.Initialize(name, config);

            // Initialisation du champs baseUri.
            _baseUri = config.MayParseValue("baseUri", _ => MayParse.ToUri(_, UriKind.RelativeOrAbsolute))
               .ValueOrThrow(() => new ProviderException("Missing or invalid config 'baseUri'."));
            config.Remove("baseUri");
        }

        // WARNING: Ne pas utiliser "/img/", par exemple car si _baseUri contient déjà un chemin 
        // relatif, il sera ignoré.

        public override Uri GetImage(string relativePath)
        {
            return MakeUri_("img/", relativePath);
        }

        public override Uri GetScript(string relativePath)
        {
            return MakeUri_("js/", relativePath);
        }

        public override Uri GetStyle(string relativePath)
        {
            return MakeUri_("css/", relativePath);
        }

        Uri MakeUri_(string basePath, string relativePath)
        {
            return new Uri(_baseUri, basePath + relativePath);
        }
    }
}
