namespace Narvalo.Web.Configuration
{
    using System.Configuration;

    public class NarvaloWebSectionGroup : ConfigurationSectionGroup
    {
        #region > Champs <

        // Nom des sections.
        internal const string GroupName = "narvalo.web";

        // Champs pour utiliser manuellement les accesseurs.
        private AssetSection _asset;
        private bool _assetSet = false;

        #endregion

        public AssetSection AssetSection
        {
            get { return _assetSet ? _asset : (AssetSection)Sections[AssetSection.DefaultName]; }
            set
            {
                _asset = value;
                _assetSet = true;
            }
        }

//        internal static string AssetSectionPath
//        {
//            get { return GetSectionPath(AssetSection.DefaultName); }
//        }
//
//        #region Private helpers
//
//        private static string GetSectionPath(string sectionName)
//        {
//            return GroupName + "/" + sectionName;
//        }
//
//        #endregion
    }
}
