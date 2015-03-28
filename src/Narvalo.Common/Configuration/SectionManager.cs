// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;

    using Narvalo.Resources;

    /// <summary>
    /// Provides methods for stronly-typed access to configuration sections.
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
        [SuppressMessage("Gendarme.Rules.Design.Generic", "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static T GetSection<T>(string sectionName) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Contract.Ensures(Contract.Result<T>() != null);

            T section = ConfigurationManager.GetSection(sectionName) as T;

            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    Format.Resource(Strings_Common.Configuration_MissingSection_Format, sectionName));
            }

            return section;
        }

        [SuppressMessage("Gendarme.Rules.Design.Generic", "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Gendarme.Rules.Portability", "MonoCompatibilityReviewRule",
            Justification = "[Intentionally] Missing method from Mono with no adequate replacement.")]
        public static T GetSection<T>(
            string sectionName,
            string configFilePath,
            ConfigurationUserLevel userLevel) where T : ConfigurationSection
        {
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Require.NotNullOrEmpty(configFilePath, "configFilePath");
            Contract.Ensures(Contract.Result<T>() != null);

            string configFileName;

            if (Path.IsPathRooted(configFilePath))
            {
                configFileName = configFilePath;
            }
            else
            {
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

            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    Format.Resource(Strings_Common.Configuration_MissingSection_Format, sectionName));
            }

            return section;
        }
    }
}
