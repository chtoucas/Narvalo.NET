namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;

    public class RemoteAssetProvider : AssetProviderBase
    {
        const string BaseUrlKey_ = "baseUrl";
        const string ImageVersionKey_ = "imageVersion";
        const string ScriptVersionKey_ = "scriptVersion";
        const string StyleVersionKey_ = "styleVersion";

        Uri _baseUrl;
        string _imageVersion;
        string _scriptVersion;
        string _styleVersion;

        public RemoteAssetProvider() : base() { }

        public Uri BaseUrl { get { return _baseUrl; } }

        public string ImageVersion { get { return _imageVersion; } }

        public string ScriptVersion { get { return _scriptVersion; } }

        public string StyleVersion { get { return _styleVersion; } }

        public override void Initialize(string name, NameValueCollection config)
        {
            Requires.NotNull(config, "config");

            if (String.IsNullOrEmpty(name)) {
                name = "VirtualAssetProvider";
            }

            if (String.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "XXX");
            }

            base.Initialize(name, config);

            // TODO vérifier que les champs sont valides.

            // Initialisation du champs _baseUrl.
            string baseUrlValue = config[BaseUrlKey_];
            if (String.IsNullOrEmpty(baseUrlValue)) {
                throw new ProviderException("XXX");
                //_baseUrl = new Uri("/");
            }
            else {
                Uri baseUrl;
                if (!Uri.TryCreate(baseUrlValue, UriKind.Absolute, out baseUrl)) {
                    throw new ProviderException("XXX");
                }
                _baseUrl = baseUrl;
            }
            config.Remove(BaseUrlKey_);

            // Initialisation du champs _scriptVersion.
            var scriptVersion = config[ScriptVersionKey_];
            if (String.IsNullOrEmpty(scriptVersion)) {
                throw new ProviderException("Empty or missing scriptVersion.");
            }
            _scriptVersion = scriptVersion;
            config.Remove(ScriptVersionKey_);

            // Initialisation du champs _styleVersion.
            var styleVersion = config[StyleVersionKey_];
            if (String.IsNullOrEmpty(styleVersion)) {
                throw new ProviderException("Empty or missing styleVersion.");
            }
            _styleVersion = styleVersion;
            config.Remove(StyleVersionKey_);

            // Initialisation du champs _imageVersion.
            var imageVersion = config[ImageVersionKey_];
            if (String.IsNullOrEmpty(imageVersion)) {
                throw new ProviderException("Empty or missing imageVersion.");
            }
            _imageVersion = imageVersion;
            config.Remove(ImageVersionKey_);

            // FIXME: On vérifie qu'il n'y a pas de champs inconnu restant.
            config.Remove("description");

            if (config.Count > 0) {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr)) {
                    throw new ProviderException("Unrecognized attribute: " + attr);
                }
            }
        }

        public override VirtualAssetFile GetImage(string relativePath)
        {
            return new RemoteImageFile(_baseUrl, relativePath, _imageVersion);
        }

        public override VirtualAssetFile GetScript(string relativePath)
        {
            return new RemoteScriptFile(_baseUrl, relativePath, _scriptVersion);
        }

        public override VirtualAssetFile GetStyle(string relativePath)
        {
            return new RemoteStyleFile(_baseUrl, relativePath, _styleVersion);
        }
    }
}
