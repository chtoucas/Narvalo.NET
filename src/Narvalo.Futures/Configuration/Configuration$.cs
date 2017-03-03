// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System.Configuration;

    using Narvalo.Fx;
    using Narvalo.Properties;

    /// <summary>
    /// Provides extension methods for <see cref="System.Configuration.Configuration"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        public static T GetSection<T>(this Configuration @this, string sectionName) where T : ConfigurationSection
        {
            Require.NotNull(@this, nameof(@this));

            T section = @this.GetSection(sectionName) as T;

            if (section == null)
            {
                throw new ConfigurationErrorsException(
                    Format.Current(Strings_Futures.Configuration_MissingSection_Format, sectionName));
            }

            return section;
        }

        public static Maybe<T> MayGetSection<T>(this Configuration @this, string sectionName)
            where T : ConfigurationSection
        {
            Require.NotNull(@this, nameof(@this));

            T section = @this.GetSection(sectionName) as T;

            return Maybe.Of(section);
        }
    }
}
