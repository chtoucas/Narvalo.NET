// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Web.WebPages;

    public static class WebPageBaseExtensions
    {
        public static HelperResult RenderSection(
            this WebPageBase @this,
            string name, 
            Func<dynamic, HelperResult> defaultContents)
        {
            if (@this.IsSectionDefined(name))
            {
                return @this.RenderSection(name);
            }

            return defaultContents(null);
        }
    }
}
