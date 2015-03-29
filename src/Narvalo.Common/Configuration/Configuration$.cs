// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Properties;

    /// <summary>
    /// Provides extension methods for <see cref="System.Configuration.Configuration"/>.
    /// </summary>
    [SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule",
        Justification = "[Intentionally] The type only contains extension methods.")]
    public static class ConfigurationExtensions
    {
        [SuppressMessage("Gendarme.Rules.Design.Generic", "AvoidMethodWithUnusedGenericTypeRule",
            Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static T GetSection<T>(this Configuration @this, string sectionName) where T : ConfigurationSection
        {
            Require.Object(@this);
            Require.NotNullOrEmpty(sectionName, "sectionName");
            Contract.Ensures(Contract.Result<T>() != null);

            T section = @this.GetSection(sectionName) as T;

            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    Format.Resource(Strings_Common.Configuration_MissingSection_Format, sectionName));
            }

            return section;
        }
    }
}
