// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System.Globalization;

    public static class RegionInfoExtensions
    {
        public static CountryISOCode GetCountryISOCode(this RegionInfo @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CountryISOCode.Of(@this.TwoLetterISORegionName);
        }
    }
}
