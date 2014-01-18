﻿namespace Narvalo.Web
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    // Cf. http://www.make-awesome.com/2012/07/perfectionist-routing-in-asp-net-mvc/
    public static class RouteCollectionExtensions
    {
        public static void MapCatchAllErrorThrowingDefaultRoute(this RouteCollection routes)
        {
            routes.Add("Default", new RestrictiveCatchAllDefaultRoute());
        }

        public class RestrictiveCatchAllDefaultRoute : Route
        {
            public RestrictiveCatchAllDefaultRoute()
                : base("*", new MvcRouteHandler())
            {
                DataTokens = new RouteValueDictionary(new { warning = "routes.MapCatchAllErrorThrowingDefaultRoute() must be the very last mapped route because it will catch everything and throw exceptions!" });
            }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
            {
                string valueDebug = String.Join(String.Empty, values.Select(p => String.Format("\r\n{0} = {1}", p.Key, p.Value)).ToArray());
                throw new InvalidOperationException("Unable to find a valid route for the following route values:" + valueDebug);
            }
        }
    }
}
