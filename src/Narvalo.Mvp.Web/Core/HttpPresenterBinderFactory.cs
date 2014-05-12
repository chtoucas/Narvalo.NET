// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;

    public static class HttpPresenterBinderFactory
    {
        public static HttpPresenterBinder Create(
            IHttpHandler httpHandler,
            HttpContext context)
        {
            return Create(new[] { httpHandler }, context, AspNetPlatformServices.Current);
        }

        public static HttpPresenterBinder Create(
            IEnumerable<Control> controls,
            HttpContext context)
        {
            return Create(controls, context, AspNetPlatformServices.Current);
        }

        public static HttpPresenterBinder Create(
            IEnumerable<object> hosts,
            HttpContext context,
            IAspNetPlatformServices platformServices)
        {
            Require.NotNull(platformServices, "platformServices");

            return new HttpPresenterBinder(
                hosts,
                context,
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                platformServices.MessageCoordinatorFactory);
        }
    }
}
