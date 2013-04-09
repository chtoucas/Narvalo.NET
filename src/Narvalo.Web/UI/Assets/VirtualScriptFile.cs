namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Globalization;

    public class VirtualScriptFile : VirtualAssetFileBase
    {
        public VirtualScriptFile(Uri baseUrl, string relativePath, string version)
            : base(baseUrl, relativePath, version) { }

        protected override string VirtualPath
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, "js/{0}/{1}", Version, RelativePath);
            }
        }
    }
}

