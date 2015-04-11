// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System.Configuration;

    public sealed class NarvaloWebSectionGroup : ConfigurationSectionGroup
    {
        public const string GroupName = "narvalo.web";

        private AssetSection _asset;
        private OptimizationSection _optimization;

        public AssetSection AssetSection
        {
            get
            {
                return _asset ?? (AssetSection)Sections[AssetSection.DefaultName];
            }

            set
            {
                Require.Property(value);

                _asset = value;
            }
        }

        public OptimizationSection OptimizationSection
        {
            get
            {
                return _optimization ?? (OptimizationSection)Sections[OptimizationSection.DefaultName];
            }

            set
            {
                Require.Property(value);

                _optimization = value;
            }
        }
    }
}
