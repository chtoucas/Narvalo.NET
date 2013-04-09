namespace Narvalo.Web.Configuration
{
    using System.Configuration;

    public class NarvaloWebSectionGroup : ConfigurationSectionGroup
    {
        #region Fields

        // Nom des sections.
        internal const string GroupName = "narvalo.web";

        // Champs pour utiliser manuellement les accesseurs.
        private AssetSection _asset;
        private bool _assetSet = false;
        private FacebookSection _facebook;
        private bool _facebookSet = false;
        private GoogleSection _google;
        private bool _googleSet = false;

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

        public FacebookSection FacebookSection
        {
            get { return _facebookSet ? _facebook : (FacebookSection)Sections[FacebookSection.DefaultName]; }
            set
            {
                _facebook = value;
                _facebookSet = true;
            }
        }

        public GoogleSection GoogleSection
        {
            get { return _googleSet ? _google : (GoogleSection)Sections[GoogleSection.DefaultName]; }
            set
            {
                _google = value;
                _googleSet = true;
            }
        }

//        internal static string AssetSectionPath
//        {
//            get { return GetSectionPath(AssetSection.DefaultName); }
//        }
//
//        internal static string FacebookSectionPath
//        {
//            get { return GetSectionPath(FacebookSection.DefaultName); }
//        }
//
//        internal static string GoogleSectionPath
//        {
//            get { return GetSectionPath(GoogleSection.DefaultName); }
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
