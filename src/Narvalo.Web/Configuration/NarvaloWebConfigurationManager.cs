namespace Narvalo.Web.Configuration
{
    using System.Configuration;
    using System.Web.Configuration;

    public static class NarvaloWebConfigurationManager
    {
        //public static NarvaloWebSectionGroup GetSectionGroup()
        //{
        //    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        
        //    return config.GetSectionGroup(NarvaloWebSectionGroup.GroupName) as NarvaloWebSectionGroup;
        //}

        public static NarvaloWebSectionGroup GetSectionGroup(Configuration config)
        {
            Requires.NotNull(config, "config");
            
            return config.SectionGroups[NarvaloWebSectionGroup.GroupName] as NarvaloWebSectionGroup;
        }

        //// FIXME
        //public static NarvaloWebSectionGroup GetSectionGroup()
        //{
        //    return WebConfigurationManager.GetSection("narvalo.web") as NarvaloWebSectionGroup;
        //}

        //public static NarvaloWebSectionGroup GetSectionGroup(string virtualPath)
        //{
        //  return new NarvaloWebSectionGroup() {
        //      AssetManager = GetAssetSection(),
        //		Facebook = GetFacebookSection(),
        //		Google = GetGoogleSection(),
        //  };
        //}

        public static AssetSection GetAssetSection()
        {
            return WebConfigurationManager<AssetSection>.GetSection(AssetSection.DefaultName);
        }

//        public static FacebookSection GetFacebookSection()
//        {
//            return WebConfigurationManager<FacebookSection>
//                .GetSection(NarvaloWebSectionGroup.FacebookSectionPath);
//        }
//
//        public static GoogleSection GetGoogleSection()
//        {
//            return WebConfigurationManager<GoogleSection>
//                .GetSection(NarvaloWebSectionGroup.GoogleSectionPath);
//        }
    }
}
