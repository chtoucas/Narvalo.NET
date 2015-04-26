// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Provides extension methods for <see cref="RouteCollection"/>.
    /// </summary>
    public static class RouteCollectionExtensions
    {
        // Cf. http://www.make-awesome.com/2012/07/perfectionist-routing-in-asp-net-mvc/
        public static void MapChildOnlyActionRoutesFrom(this RouteCollection routes, Assembly assembly)
        {
            Require.NotNull(assembly, "assembly");

            var controllerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(IController).IsAssignableFrom(t));

            foreach (Type t in controllerTypes)
            {
                string controllerName = t.Name;

                // Le nom du controleur Mvc finit toujours en "Controller".
                if (controllerName.Length < 10)
                {
                    continue;
                }

                controllerName = controllerName.Substring(0, controllerName.Length - 10);

                bool childOnlyController = t.GetCustomAttributes(typeof(ChildActionOnlyAttribute), inherit: true).Any();

                foreach (MethodInfo m in t.GetMethods())
                {
                    if (m.IsPublic && typeof(ActionResult).IsAssignableFrom(m.ReturnType))
                    {
                        if (!childOnlyController)
                        {
                            bool childOnlyAction = m.GetCustomAttributes(typeof(ChildActionOnlyAttribute), inherit: true).Any();

                            if (!childOnlyAction)
                            {
                                continue;
                            }
                        }

                        string actionName = m.Name;
                        string name = "ChildAction/" + controllerName + "/" + actionName;
                        var constraints = new { controller = controllerName, action = actionName };

                        routes.MapRoute(name, String.Empty, null /* defaults */, constraints);
                    }
                }
            }
        }
    }
}
