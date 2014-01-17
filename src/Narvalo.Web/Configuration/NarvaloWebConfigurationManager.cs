namespace Narvalo.Web.Configuration
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Configuration;

    public static class NarvaloWebConfigurationManager
    {
        public static NarvaloWebSectionGroup GetSectionGroup()
        {
            return GetSectionGroup("/");
        }

        public static NarvaloWebSectionGroup GetSectionGroup(string virtualPath)
        {
            return WebConfigurationManager.OpenWebConfiguration(virtualPath)
                .GetSectionGroup(NarvaloWebSectionGroup.GroupName) as NarvaloWebSectionGroup;
        }

        public static NarvaloWebSectionGroup GetSectionGroup(Configuration config)
        {
            Requires.NotNull(config, "config");
            
            return config.SectionGroups[NarvaloWebSectionGroup.GroupName] as NarvaloWebSectionGroup;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "La valeur retournée peut changer entre deux appels.")]
        public static AssetSection GetAssetSection()
        {
            return WebConfigurationManager<AssetSection>.GetSection(AssetSection.SectionName);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "La valeur retournée peut changer entre deux appels.")]
        public static OptimizationSection GetOptimizationSection()
        {
            return WebConfigurationManager<OptimizationSection>.GetSection(OptimizationSection.SectionName);
        }
    }
}
