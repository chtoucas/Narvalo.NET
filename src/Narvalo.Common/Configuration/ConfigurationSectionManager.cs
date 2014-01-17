namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// Fournit des méthodes permettant d'accéder aux fichiers de configuration.
    /// </summary>
    public static class ConfigurationSectionManager
    {
        /// <summary>
        /// Récupére une section pour la configuration par défaut de l'application courante.
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
                throw Failure.ConfigurationErrors(SR.ConfigurationManager_MissingSectionFormat, sectionName);
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

                configFileName = Path.Combine(configurationDirectory, configFilePath);
            }

            var map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configFileName;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, userLevel);
            var section = configuration.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.ConfigurationManager_MissingSectionFormat, sectionName);
            }

            return section;
        }
    }
}
