namespace Narvalo.Web
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    // Cf. http://www.make-awesome.com/2012/07/perfectionist-routing-in-asp-net-mvc/
    public static class RouteCollectionExtensions
    {
        public static void MapChildOnlyActionRoutesFrom(this RouteCollection routes, Assembly assembly)
        {
            var controllerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(IController).IsAssignableFrom(t));

            foreach (Type t in controllerTypes) {
                bool childOnlyController = t.GetCustomAttributes(typeof(ChildActionOnlyAttribute), true /* inherit */).Any();

                foreach (MethodInfo m in t.GetMethods()) {
                    if (m.IsPublic && typeof(ActionResult).IsAssignableFrom(m.ReturnType)) {
                        if (!childOnlyController) {
                            bool childOnlyAction = m.GetCustomAttributes(typeof(ChildActionOnlyAttribute), true).Any();

                            if (!childOnlyAction) { continue; }
                        }

                        string controllerName = t.Name;
                        string actionName = m.Name;

                        // A controller's name always end with "Controller".
                        ////if (controllerName.EndsWith("Controller")) {
                        controllerName = controllerName.Substring(0, controllerName.Length - 10);
                        ////}

                        string name = String.Format(CultureInfo.InvariantCulture, "ChildAction/{0}/{1}", controllerName, actionName);
                        var constraints = new { controller = controllerName, action = actionName };

                        routes.MapRoute(name, String.Empty, null /* defaults */, constraints);
                    }
                }
            }
        }

        ////public static void MapCatchAllErrorThrowingDefaultRoute(this RouteCollection routes)
        ////{
        ////    routes.Add("Default", new RestrictiveCatchAllDefaultRoute());
        ////}

        ////public class RestrictiveCatchAllDefaultRoute : Route
        ////{
        ////    public RestrictiveCatchAllDefaultRoute()
        ////        : base("*", new MvcRouteHandler())
        ////    {
        ////        DataTokens = new RouteValueDictionary(new { warning = "routes.MapCatchAllErrorThrowingDefaultRoute() must be the very last mapped route because it will catch everything and throw exceptions!" });
        ////    }

        ////    public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        ////    {
        ////        string valueDebug = String.Join(String.Empty, values.Select(p => String.Format("\r\n{0} = {1}", p.Key, p.Value)).ToArray());
        ////        throw new InvalidOperationException("Unable to find a valid route for the following route values:" + valueDebug);
        ////    }
        ////}
    }
}
