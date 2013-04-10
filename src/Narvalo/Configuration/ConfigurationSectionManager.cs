namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using System.IO;
    using Narvalo.Resources;

    public static class ConfigurationSectionManager
    {
        //static readonly string ConfigurationDirectory 
        //    = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");

            T section = ConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw Fault.ConfigurationErrors(
                    SR.Configuration_MissingSection,
                    sectionName);
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
                throw Fault.ConfigurationErrors(
                    SR.Configuration_MissingSection,
                    sectionName);
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

            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configFilename;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, userLevel);
            var section = configuration.GetSection(sectionName) as T;

            if (section == null) {
                throw Fault.ConfigurationErrors(
                    SR.Configuration_MissingSection,
                    sectionName);
            }

            return section;
        }
    }
    //
    //    public static class ConfigurationManager<T> where T : ConfigurationSection
    //    {
    //        //private static readonly string ConfigurationDirectory 
    //        //    = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
    //
    //        public static T GetSection(string sectionName)
    //        {
    //            Requires.NotNullOrEmpty(sectionName, "sectionName");
    //
    //            T section = ConfigurationManager.GetSection(sectionName) as T;
    //
    //            if (section == null) {
    //                throw new ConfigurationErrorsException(
    //                    Failure.Format(
    //                        ConfigurationMessages.MissingSection,
    //                        sectionName));
    //            }
    //
    //            return section;
    //        }
    //        
    //        public static T GetSection(Configuration config, string sectionName)
    //        {
    //            Requires.NotNull(config, "config");
    //            Requires.NotNullOrEmpty(sectionName, "sectionName");
    //
    //            T section = config.GetSection(sectionName) as T;
    //
    //            if (section == null) {
    //                throw new ConfigurationErrorsException(
    //                    Failure.Format(
    //                        ConfigurationMessages.MissingSection,
    //                        sectionName));
    //            }
    //
    //            return section;
    //        }
    //
    //        public static T GetSection(string sectionName, string configFilePath, ConfigurationUserLevel userLevel)
    //        {
    //            Requires.NotNullOrEmpty(sectionName, "sectionName");
    //            Requires.NotNullOrEmpty(configFilePath, "configFilePath");
    //
    //            string configFilename;
    //            if (Path.IsPathRooted(configFilePath)) {
    //                configFilename = configFilePath;
    //            }
    //            else {
    //                string configurationDirectory 
    //                    = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
    //
    //                configFilename = Path.Combine(configurationDirectory, configFilePath);
    //            }
    //
    //            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
    //            map.ExeConfigFilename = configFilename;
    //
    //            var configuration = ConfigurationManager.OpenMappedExeConfiguration(map, userLevel);
    //            var section = configuration.GetSection(sectionName) as T;
    //
    //            if (section == null) {
    //                throw new ConfigurationErrorsException(
    //                    Failure.Format(
    //                        ConfigurationMessages.MissingSection,
    //                        sectionName));
    //            }
    //
    //            return section;
    //        }
    //    }

}
