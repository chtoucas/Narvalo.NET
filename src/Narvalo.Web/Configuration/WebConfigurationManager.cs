namespace Narvalo.Web.Configuration
{
    using System.Configuration;
    using System.Web.Configuration;

    public static class WebConfigurationSectionManager
    {
        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");

            T section = WebConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.WebConfigurationManager_SectionNotFoundFormat, sectionName);
            }

            return section;
        }

        public static T GetSection<T>(string sectionName, string virtualPath) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            T section = WebConfigurationManager.GetSection(sectionName, virtualPath) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.WebConfigurationManager_SectionNotFoundInPathFormat, sectionName, virtualPath);
            }

            return section;
        }
    }
}
