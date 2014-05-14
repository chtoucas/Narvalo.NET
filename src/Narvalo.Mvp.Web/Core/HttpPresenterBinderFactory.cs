﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using Narvalo.Mvp.Platforms;

    public static class HttpPresenterBinderFactory
    {
        public static HttpPresenterBinder Create(
            MvpHttpHandler httpHandler,
            HttpContext context)
        {
            return Create(
                new[] { httpHandler },
                context,
                PlatformServices.Current,
                MessageCoordinatorBlackHole.Instance);
        }

        public static HttpPresenterBinder Create(
            IEnumerable<Control> controls,
            HttpContext context)
        {
            return Create(
                controls,
                context,
                PlatformServices.Current,
                PlatformServices.Current.MessageCoordinatorFactory.Create());
        }

        internal static HttpPresenterBinder Create(
            IEnumerable<object> hosts,
            HttpContext context,
            IPlatformServices platformServices,
            IMessageCoordinator messageCoordinator)
        {
            DebugCheck.NotNull(platformServices);

            return new HttpPresenterBinder(
                hosts,
                context,
                platformServices.PresenterDiscoveryStrategy,
                platformServices.PresenterFactory,
                platformServices.CompositeViewFactory,
                messageCoordinator);
        }
    }
}
