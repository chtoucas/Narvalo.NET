// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;
    using System.Web.Configuration;

    public static class NarvaloWebConfigurationManager
    {
        private static readonly Lazy<OptimizationSection> s_OptimizationSection
            = new Lazy<OptimizationSection>(InitializeOptimizationSection);

        internal static OptimizationSection OptimizationSection
        {
            get
            {
                Warrant.NotNull<OptimizationSection>();

                return s_OptimizationSection.Value;
            }
        }

        public static NarvaloWebSectionGroup GetSectionGroup()
        {
            return GetSectionGroup("/");
        }

        public static NarvaloWebSectionGroup GetSectionGroup(string virtualPath)
        {
            var config = WebConfigurationManager.OpenWebConfiguration(virtualPath);

            if (config == null) { return null; }

            return config.GetSectionGroup(NarvaloWebSectionGroup.GroupName) as NarvaloWebSectionGroup;
        }

        public static NarvaloWebSectionGroup GetSectionGroup(Configuration config)
        {
            Require.NotNull(config, nameof(config));

            return config.SectionGroups[NarvaloWebSectionGroup.GroupName] as NarvaloWebSectionGroup;
        }

        private static OptimizationSection InitializeOptimizationSection()
        {
            Warrant.NotNull<OptimizationSection>();

            var section = WebConfigurationManager.GetSection(
                Narvalo.Web.Configuration.OptimizationSection.SectionName) as OptimizationSection;

            return section ?? new OptimizationSection();
        }
    }
}
