﻿namespace Narvalo.Web.Configuration
{
    using System.Configuration;
    using System.Web.Configuration;

    public static class WebConfigurationManager<T> where T : ConfigurationSection
    {
        public static T GetSection(string sectionName)
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");

            T section = WebConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.WebConfigurationManager_SectionNotFoundFormat, sectionName);
            }

            return section;
        }

        public static T GetSection(string sectionName, string virtualPath)
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");
            Requires.NotNullOrEmpty(virtualPath, "virtualPath");

            T section = WebConfigurationManager.GetSection(sectionName, virtualPath) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.WebConfigurationManager_SectionNotFoundInPathFormat, sectionName, virtualPath);
            }

            return section;
        }
    }
}
