namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;

    public static class ConfigurationManager<T> where T : ConfigurationSection
    {
        public static T GetSection(string sectionName)
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");

            T section = ConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw Failure.ConfigurationErrors(SR.ConfigurationManager_MissingSectionFormat, sectionName);
            }

            return section;
        }

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
