namespace Narvalo.Web.UI.Assets
{
    using System;

    public class VirtualAssetFile
    {
        readonly string _relativePath;

        Uri _url;

        public VirtualAssetFile(string relativePath)
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
                    _url = new Uri(_relativePath, UriKind.Relative);
                }
                return _url;
            }
        }
    }
}

