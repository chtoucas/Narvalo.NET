// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.Contracts;

    public sealed partial class OptimizationSection : ConfigurationSection
    {
        public const string DefaultName = "optimization";

        public static readonly string SectionName = NarvaloWebSectionGroup.GroupName + "/" + DefaultName;

        internal const string EnableWhiteSpaceBustingPropertyName = "enableWhiteSpaceBusting";

        private const bool ENABLE_WHITESPACE_BUSTING_DEFAUL_VALUE = false;

        private static readonly ConfigurationPropertyCollection s_Properties;

        private static readonly ConfigurationProperty s_EnableWhiteSpaceBusting
            = new ConfigurationProperty(
                EnableWhiteSpaceBustingPropertyName,
                typeof(Boolean),
                ENABLE_WHITESPACE_BUSTING_DEFAUL_VALUE,
                ConfigurationPropertyOptions.None);

        private bool _enableWhiteSpaceBusting;
        private bool _enableWhiteSpaceBustingSet;

        static OptimizationSection()
        {
            s_Properties = new ConfigurationPropertyCollection();
            s_Properties.Add(s_EnableWhiteSpaceBusting);
        }

        public bool EnableWhiteSpaceBusting
        {
            get
            {
                if (_enableWhiteSpaceBustingSet)
                {
                    return _enableWhiteSpaceBusting;
                }
                else
                {
                    // I think that ConfigurationSection (or more precisely ConfigurationElement)
                    // guarantees that value is never null, but we never know.
                    var value = base[s_EnableWhiteSpaceBusting];

                    return value == null
                        ? ENABLE_WHITESPACE_BUSTING_DEFAUL_VALUE
                        : (bool)base[s_EnableWhiteSpaceBusting];
                }
            }

            set
            {
                _enableWhiteSpaceBusting = value;
                _enableWhiteSpaceBustingSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                Warrant.NotNull<ConfigurationPropertyCollection>();

                return s_Properties;
            }
        }
    }
}
