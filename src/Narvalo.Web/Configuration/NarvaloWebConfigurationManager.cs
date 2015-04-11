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
            = new Lazy<OptimizationSection>(InitializeOptimizationSection_);

        internal static OptimizationSection OptimizationSection
        {
            get
            {
                Contract.Ensures(Contract.Result<OptimizationSection>() != null);

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
            Require.NotNull(config, "config");

            return config.SectionGroups[NarvaloWebSectionGroup.GroupName] as NarvaloWebSectionGroup;
        }

        private static OptimizationSection InitializeOptimizationSection_()
        {
            Contract.Ensures(Contract.Result<OptimizationSection>() != null);

            var section = WebConfigurationManager.GetSection(
                Narvalo.Web.Configuration.OptimizationSection.SectionName) as OptimizationSection;

            return section ?? new OptimizationSection();
        }
    }
}
