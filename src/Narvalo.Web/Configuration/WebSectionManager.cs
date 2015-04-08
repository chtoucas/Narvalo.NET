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
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Contract.Ensures(Contract.Result<T>() != null);

            T section = WebConfigurationManager.GetSection(sectionName) as T;

            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    Format.Resource(Strings_Web.WebConfigurationManager_SectionNotFound_Format, sectionName));
            }

            return section;
        }

        public static T GetSection<T>(string sectionName, string virtualPath) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Require.NotNullOrEmpty(virtualPath, "virtualPath");
            Contract.Ensures(Contract.Result<T>() != null);

            T section = WebConfigurationManager.GetSection(sectionName, virtualPath) as T;

            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    Format.Resource(
                        Strings_Web.WebConfigurationManager_SectionNotFoundInPath_Format,
                        sectionName,
                        virtualPath));
            }

            return section;
        }

        public static Maybe<T> MayGetSection<T>(string sectionName) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            T section = WebConfigurationManager.GetSection(sectionName) as T;

            return Maybe.Of(section);
        }

        public static Maybe<T> MayGetSection<T>(string sectionName, string virtualPath) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Require.NotNullOrEmpty(virtualPath, "virtualPath");
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            T section = WebConfigurationManager.GetSection(sectionName, virtualPath) as T;

            return Maybe.Of(section);
        }
    }
}
