// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;

    public static class NarvaloWebConfigurationManager
    {
        private static readonly Lazy<AssetSection> AssetSection_ = new Lazy<AssetSection>(InitializeAssetSection_);
        private static readonly Lazy<OptimizationSection> OptimizationSection_ = new Lazy<OptimizationSection>(InitializeOptimizationSection_);

        public static AssetSection AssetSection { get { return AssetSection_.Value; } }

        public static OptimizationSection OptimizationSection { get { return OptimizationSection_.Value; } }

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
            Require.NotNull(config, "config");

            return config.SectionGroups[NarvaloWebSectionGroup.GroupName] as NarvaloWebSectionGroup;
        }

        private static AssetSection InitializeAssetSection_()
        {
            return WebSectionManager.GetSection<AssetSection>(AssetSection.SectionName);
        }

        private static OptimizationSection InitializeOptimizationSection_()
        {
            return WebSectionManager.GetSection<OptimizationSection>(OptimizationSection.SectionName);
        }
    }
}
