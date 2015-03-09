// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System.Globalization;

    public static class RegionInfoExtensions
    {
        public static short GetISOCountryCode(this RegionInfo @this)
        {
            return Region.GetISOCode(@this.TwoLetterISORegionName);
        }
    }
}
