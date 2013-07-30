namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Globalization;

    public class RemoteStyleFile : RemoteAssetFileBase
    {
        public RemoteStyleFile(Uri baseUrl, string relativePath, string version)
            : base(baseUrl, relativePath, version) { }

        protected override string VirtualPath
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, "css/{0}/{1}", Version, RelativePath);
            }
        }
    }
}

