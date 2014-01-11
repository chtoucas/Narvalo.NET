namespace Narvalo.Web.UI.Assets
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class AssetBundle
    {
        readonly string _bundlePath;

        IList<string> _relativePaths = new List<string>();

        public AssetBundle(string bundlePath)
        {
            Requires.NotNullOrEmpty(bundlePath, "bundlePath");

            _bundlePath = bundlePath;
        }

        public string BundlePath { get { return _bundlePath; } }

        public IReadOnlyCollection<string> RelativePaths
        {
            get { return new ReadOnlyCollection<string>(_relativePaths); }
        }

        public static AssetBundle Create(string bundlePath)
        {
            return new AssetBundle(bundlePath);
        }

        public AssetBundle Include(string relativePath)
        {
            Requires.NotNullOrEmpty(relativePath, "relativePath");

            _relativePaths.Add(relativePath);

            return this;
        }
    }
}
