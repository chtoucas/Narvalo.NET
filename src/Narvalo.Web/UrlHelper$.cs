namespace Narvalo.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Narvalo;

    public static class UrlHelperExtensions
    {
        public static string Current(this UrlHelper @this)
        {
            Requires.NotNull(@this, "this");

            return @this.RequestContext.HttpContext.Request.RawUrl;
        }

        public static string AbsoluteAction(this UrlHelper @this, string actionName, string controllerName, object routeValues)
        {
            Requires.NotNull(@this, "this");

            // NB: En ajoutant le paramètre scheme, on obtient une URL absolue.
            var scheme = @this.RequestContext.HttpContext.Request.Url.Scheme;
            return @this.Action(actionName, controllerName, routeValues, scheme);
        }

        public static string AbsoluteAction(this UrlHelper @this, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            Requires.NotNull(@this, "this");

            // NB: En ajoutant le paramètre scheme, on obtient une URL absolue.
            var scheme = @this.RequestContext.HttpContext.Request.Url.Scheme;
            return @this.Action(actionName, controllerName, routeValues, scheme);
        }

        // Voir aussi http://weblog.west-wind.com/posts/2007/Sep/18/ResolveUrl-without-Page
        public static string AbsoluteContent(this UrlHelper self, string path)
        {
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);

            if (!uri.IsAbsoluteUri) {
                Uri requestUrl = self.RequestContext.HttpContext.Request.Url;
                var builder = new UriBuilder(requestUrl.Scheme, requestUrl.Host, requestUrl.Port);

                builder.Path = VirtualPathUtility.ToAbsolute(path);
                uri = builder.Uri;
            }

            return uri.ToString();
        }
    }
}
