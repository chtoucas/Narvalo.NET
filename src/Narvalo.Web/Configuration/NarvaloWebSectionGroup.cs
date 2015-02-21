// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System.Configuration;

    public sealed class NarvaloWebSectionGroup : ConfigurationSectionGroup
    {
        public const string GroupName = "narvalo.web";

        private AssetSection _asset;
        private OptimizationSection _optimization;

        private bool _assetSet = false;
        private bool _optimizationSet = false;

        public AssetSection AssetSection
        {
            get { 
                return _assetSet ? _asset : (AssetSection)Sections[AssetSection.DefaultName];
            }
            
            set { 
                _asset = value;
                _assetSet = true;
            }
        }

        public OptimizationSection OptimizationSection
        {
            get {
                return _optimizationSet ? _optimization : (OptimizationSection)Sections[OptimizationSection.DefaultName];
            }
            
            set { 
                _optimization = value; 
                _optimizationSet = true;
            }
        }
    }
}
