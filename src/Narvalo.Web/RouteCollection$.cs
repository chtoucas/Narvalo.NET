namespace Narvalo.Web
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteCollectionExtensions
    {
        [Alien(AlienSource.Informal,
            Link = "http://www.make-awesome.com/2012/07/perfectionist-routing-in-asp-net-mvc/")]
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

                        // Le nom du controleur Mvc finit toujours en "Controller".
                        controllerName = controllerName.Substring(0, controllerName.Length - 10);

                        string name = String.Format(CultureInfo.InvariantCulture, "ChildAction/{0}/{1}", controllerName, actionName);
                        var constraints = new { controller = controllerName, action = actionName };

                        routes.MapRoute(name, String.Empty, null /* defaults */, constraints);
                    }
                }
            }
        }
    }
}
