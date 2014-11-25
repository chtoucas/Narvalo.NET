// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System.Configuration;
    using System.Web.Configuration;

    public static class WebSectionManager
    {
        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");

            T section = WebConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw new ConfigurationErrorsException(
                    Format.CurrentCulture(Strings_Web.WebConfigurationManager_SectionNotFoundFormat, sectionName));
            }

            return section;
        }

        public static T GetSection<T>(string sectionName, string virtualPath) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Require.NotNullOrEmpty(virtualPath, "virtualPath");

            T section = WebConfigurationManager.GetSection(sectionName, virtualPath) as T;

            if (section == null) {
                throw new ConfigurationErrorsException(
                    Format.CurrentCulture(Strings_Web.WebConfigurationManager_SectionNotFoundInPathFormat, sectionName, virtualPath));
            }

            return section;
        }
    }
}
