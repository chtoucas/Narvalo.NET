﻿namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
    /// Fournit des méthodes permettant d'accéder aux fichiers de configuration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ConfigurationManager<T> where T : ConfigurationSection
    {
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Une version non générique n'améliorerait pas la compréhension de la méthode.")]
        public static T GetSection(string sectionName)
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");

            T section = ConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.ConfigurationManager_MissingSectionFormat, sectionName);
            }

            return section;
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Une version non générique n'améliorerait pas la compréhension de la méthode.")]
        public static T GetSection(Configuration config, string sectionName)
        {
            Requires.NotNull(config, "config");
            Requires.NotNullOrEmpty(sectionName, "sectionName");

            T section = config.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.ConfigurationManager_MissingSectionFormat, sectionName);
            }

            return section;
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Une version non générique n'améliorerait pas la compréhension de la méthode.")]
        public static T GetSection(
            string sectionName,
            string configFilePath,
            ConfigurationUserLevel userLevel)
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");
            Requires.NotNullOrEmpty(configFilePath, "configFilePath");

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
