namespace Narvalo.Web.UI.Assets
{
    using System;

    public abstract class VirtualAssetFileBase : AssetFile
    {
        readonly Uri _baseUrl;
        readonly string _version;

        Uri _url;

        protected VirtualAssetFileBase(Uri baseUrl, string relativePath, string version)
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

        //[SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public override Uri Url
        {
            get
            {
                if (_url == null) {
                    // FIXME: http://stackoverflow.com/questions/372865/path-combine-for-urls
                    // http://stackoverflow.com/questions/4925468/combine-relative-baseuri-with-relative-path
                    _url = _baseUrl.IsAbsoluteUri ? new Uri(_baseUrl, VirtualPath) : new Uri(VirtualPath);
                }

                return _url;
            }
        }

        protected abstract string VirtualPath { get; }
    }
}

