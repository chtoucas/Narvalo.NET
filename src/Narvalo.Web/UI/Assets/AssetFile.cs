namespace Narvalo.Web.UI.Assets
{
    using System;

    public class AssetFile
    {
        readonly string _relativePath;

        Uri _url;

        public AssetFile(string relativePath)
            : base()
        {
            Requires.NotNullOrEmpty(relativePath, "relativePath");

            _relativePath = relativePath;
        }

        public string RelativePath
        {
            get { return _relativePath; }
        }

        public virtual Uri Url
        {
            get
            {
                if (_url == null) {
                    _url = new Uri(_relativePath);
                }
                return _url;
            }
        }
    }
}

