// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Web.Configuration;

    using Narvalo.Fx;
    using Narvalo.Web.Properties;

    public static class WebSectionManager
    {
        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            Warrant.NotNull<T>();

            T section = WebConfigurationManager.GetSection(sectionName) as T;

            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    Format.Current(Strings.WebConfigurationManager_SectionNotFound_Format, sectionName));
            }

            return section;
        }

        public static T GetSection<T>(string sectionName, string virtualPath) where T : ConfigurationSection
        {
            Warrant.NotNull<T>();

            T section = WebConfigurationManager.GetSection(sectionName, virtualPath) as T;

            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    Format.Current(
                        Strings.WebConfigurationManager_SectionNotFoundInPath_Format,
                        sectionName,
                        virtualPath));
            }

            return section;
        }

        public static Maybe<T> MayGetSection<T>(string sectionName) where T : ConfigurationSection
        {
            T section = WebConfigurationManager.GetSection(sectionName) as T;

            return Maybe.Of(section);
        }

        public static Maybe<T> MayGetSection<T>(string sectionName, string virtualPath) where T : ConfigurationSection
        {
            T section = WebConfigurationManager.GetSection(sectionName, virtualPath) as T;

            return Maybe.Of(section);
        }
    }
}
