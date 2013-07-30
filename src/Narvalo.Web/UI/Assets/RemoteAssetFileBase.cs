namespace Narvalo.Web.UI.Assets
{
    using System;

    public abstract class RemoteAssetFileBase : VirtualAssetFile
    {
        readonly Uri _baseUrl;
        readonly string _version;

        Uri _url;

        protected RemoteAssetFileBase(Uri baseUrl, string relativePath, string version)
            : base(relativePath)
        {
            Requires.NotNull(baseUrl, "baseUrl");
            Requires.NotNullOrEmpty(version, "version");

            _baseUrl = baseUrl;
            _version = version;
        }

        public string Version
        {
            get { return _version; }
        }

        public override Uri Url
        {
            get
            {
                if (_url == null) {
                    _url = new Uri(_baseUrl, VirtualPath);
                }

                return _url;
            }
        }

        protected abstract string VirtualPath { get; }
    }
}

