// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Web.WebPages;

    public static class WebPageBaseExtensions
    {
        public static HelperResult RenderSection(this WebPageBase webPage,
            string name, Func<dynamic, HelperResult> defaultContents)
        {
            if (webPage.IsSectionDefined(name)) {
                return webPage.RenderSection(name);
            }
            return defaultContents(null);
        }
    }
}
