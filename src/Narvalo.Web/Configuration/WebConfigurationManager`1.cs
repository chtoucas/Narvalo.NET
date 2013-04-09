namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Web.Configuration;

    public static class WebConfigurationManager<T> where T : ConfigurationSection
    {
        public static T GetSection(string sectionName)
        {
            Requires.NotNullOrEmpty(sectionName, "sectionName");

            T section = WebConfigurationManager.GetSection(sectionName) as T;

            if (section == null) {
                throw new ConfigurationErrorsException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "The <{0}> section is not defined in your web.config!",
                        sectionName));
            }

            return section;
        }
    }
}
