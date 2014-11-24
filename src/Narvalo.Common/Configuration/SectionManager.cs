// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// Provides methods to stronly-typed access to configuration sections.
    /// </summary>
    public static class SectionManager
    {
        /// <summary>
        /// Récupère une section pour la configuration par défaut de l'application courante.
        /// </summary>
        /// <typeparam name="T">La classe représentant la section de configuration.</typeparam>
        /// <param name="sectionName">Nom de la section.</param>
        /// <returns>La section de configuration.</returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">La section n'a pas été trouvée ou n'est pas du type demandée.</exception>>
        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");

            T section = ConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw new ConfigurationErrorsException(
                    Format.CurrentCulture(SR.ConfigurationManager_MissingSectionFormat, sectionName));
            }

            return section;
        }

        public static T GetSection<T>(
            string sectionName,
            string configFilePath,
            ConfigurationUserLevel userLevel) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Require.NotNullOrEmpty(configFilePath, "configFilePath");

            string configFileName;

            if (Path.IsPathRooted(configFilePath)) {
                configFileName = configFilePath;
            }
            else {
                string configurationDirectory
                    = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                configFileName = configurationDirectory == null
                    ? configFilePath
                    : Path.Combine(configurationDirectory, configFilePath);
            }

            var map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configFileName;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, userLevel);
            var section = configuration.GetSection(sectionName) as T;

            if (section == null) {
                throw new ConfigurationErrorsException(
                    Format.CurrentCulture(SR.ConfigurationManager_MissingSectionFormat, sectionName));
            }

            return section;
        }
    }
}
