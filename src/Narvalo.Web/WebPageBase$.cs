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