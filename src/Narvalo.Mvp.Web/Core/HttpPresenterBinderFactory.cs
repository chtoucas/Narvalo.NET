// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
    using System.Web;

    public static class HttpPresenterBinderFactory
    {
        public static HttpPresenterBinder Create(
            object host,
            HttpContext context)
        {
            return Create(new[] { host }, context, AspNetPlatformServices.Current);
        }

        public static HttpPresenterBinder Create(
            IEnumerable<object> hosts,
            HttpContext context)
        {
            return Create(hosts, context, AspNetPlatformServices.Current);
        }

        public static HttpPresenterBinder Create(
            IEnumerable<object> hosts,
            HttpContext context,
            IAspNetPlatformServices container)
        {
            Require.NotNull(container, "container");

            return new HttpPresenterBinder(
                hosts,
                context,
                container.PresenterDiscoveryStrategy,
                container.PresenterFactory,
                container.CompositeViewFactory,
                container.MessageCoordinatorFactory);
        }
    }
}
