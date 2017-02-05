// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Web.Mvc;

    public static class HttpVerbsExtensions
    {
        public static bool Contains(this HttpVerbs @this, HttpVerbs value) => (@this & value) != 0;
    }
}
