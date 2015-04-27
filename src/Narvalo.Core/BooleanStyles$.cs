// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    public static class BooleanStylesExtensions
    {
        public static bool Contains(this BooleanStyles @this, BooleanStyles value)
        {
            return (@this & value) != 0;
        }
    }
}
