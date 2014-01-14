namespace Narvalo.Web.Configuration
{
    using System.Configuration;

    public class NarvaloWebSectionGroup : ConfigurationSectionGroup
    {
        public const string GroupName = "narvalo.web";

        AssetSection _asset;
        OptimizationSection _optimization;

        bool _assetSet = false;
        bool _optimizationSet = false;

        public AssetSection AssetSection
        {
            get { return _assetSet ? _asset : (AssetSection)Sections[AssetSection.DefaultName]; }
            set { _asset = value; _assetSet = true; }
        }

        public OptimizationSection OptimizationSection
        {
            get { return _optimizationSet ? _optimization : (OptimizationSection)Sections[OptimizationSection.DefaultName]; }
            set { _optimization = value; _optimizationSet = true; }
        }
    }
}
