// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public static class HttpPresenterBinderFactory
    {
        public static HttpPresenterBinder Create(
            object host,
            HttpContext context)
        {
            throw new NotImplementedException();
        }

        public static HttpPresenterBinder Create(
            IEnumerable<object> hosts,
            HttpContext context)
        {
            throw new NotImplementedException();
        }

        public static HttpPresenterBinder Create(
            IEnumerable<object> hosts,
            HttpContext context,
            IAspNetServicesContainer container)
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
