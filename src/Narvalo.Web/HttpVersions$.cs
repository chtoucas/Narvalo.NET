// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    public static class HttpVersionsExtensions
    {
        public static bool Contains(this HttpVersions @this, HttpVersions value)
        {
            return (@this & value) != 0;
        }
    }
}
