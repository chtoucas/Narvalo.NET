namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;

    public static class ConfigurationSectionManager
    {
        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");

            T section = ConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.Configuration_MissingSection, sectionName);
            }

            return section;
        }

        public static T GetSection<T>(Configuration config, string sectionName)
            where T : ConfigurationSection
        {
            Requires.NotNull(config, "config");
            Requires.NotNullOrEmpty(sectionName, "sectionName");

            T section = config.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.Configuration_MissingSection, sectionName);
            }

            return section;
        }

        public static T GetSection<T>(
            string sectionName,
            string configFilePath,
            ConfigurationUserLevel userLevel)
            where T : ConfigurationSection
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");
            Requires.NotNullOrEmpty(configFilePath, "configFilePath");

            string configFilename;
            if (Path.IsPathRooted(configFilePath)) {
                configFilename = configFilePath;
            }
            else {
                string configurationDirectory
                    = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                configFilename = Path.Combine(configurationDirectory, configFilePath);
            }

            var map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configFilename;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, userLevel);
            var section = configuration.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.Configuration_MissingSection, sectionName);
            }

            return section;
        }
    }
}
