// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Provides extension methods for <see cref="UrlHelper"/>.
    /// </summary>
    public static class UrlHelperExtensions
    {
        public static string Current(this UrlHelper @this)
        {
            Require.NotNull(@this, nameof(@this));

            var requestContext = @this.RequestContext;
            Contract.Assume(requestContext != null);

            return requestContext
                .HttpContext
                .Request
                .RawUrl;
        }

        public static string AbsoluteAction(
            this UrlHelper @this,
            string actionName,
            string controllerName,
            object routeValues)
        {
            Require.NotNull(@this, nameof(@this));

            var requestContext = @this.RequestContext;
            Contract.Assume(requestContext != null);

            // NB: En ajoutant le paramètre scheme, on obtient une URL absolue.
            var requestUrl = requestContext.HttpContext.Request.Url;
            Contract.Assume(requestUrl != null);

            return @this.Action(actionName, controllerName, routeValues, requestUrl.Scheme);
        }

        public static string AbsoluteAction(
            this UrlHelper @this,
            string actionName,
            string controllerName,
            RouteValueDictionary routeValues)
        {
            Require.NotNull(@this, nameof(@this));

            var requestContext = @this.RequestContext;
            Contract.Assume(requestContext != null);

            // NB: En ajoutant le paramètre scheme, on obtient une URL absolue.
            var requestUrl = requestContext.HttpContext.Request.Url;
            Contract.Assume(requestUrl != null);

            return @this.Action(actionName, controllerName, routeValues, requestUrl.Scheme);
        }

        // Cf. http://weblog.west-wind.com/posts/2007/Sep/18/ResolveUrl-without-Page
        public static string AbsoluteContent(this UrlHelper @this, string path)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<string>();

            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);

            if (!uri.IsAbsoluteUri)
            {
                var requestContext = @this.RequestContext;
                Contract.Assume(requestContext != null);

                // NB: En ajoutant le paramètre scheme, on obtient une URL absolue.
                var requestUrl = requestContext.HttpContext.Request.Url;
                Contract.Assume(requestUrl != null);
                Contract.Assume(requestUrl.Port >= -1);

                var builder = new UriBuilder(requestUrl.Scheme, requestUrl.Host, requestUrl.Port);

                builder.Path = VirtualPathUtility.ToAbsolute(path);
                uri = builder.Uri;
            }

            return uri.ToString();
        }
    }
}
