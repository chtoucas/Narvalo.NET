namespace Narvalo.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Configuration.Configuration"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        public static T GetSection<T>(this Configuration @this, string sectionName) where T : ConfigurationSection
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(sectionName, "sectionName");

            T section = @this.GetSection(sectionName) as T;

            if (section == null) {
                throw new ConfigurationErrorsException(
                    Format.CurrentCulture(SR.ConfigurationManager_MissingSectionFormat, sectionName));
            }

            return section;
        }
    }
}
