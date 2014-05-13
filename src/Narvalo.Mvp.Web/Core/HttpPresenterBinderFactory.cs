// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using Narvalo.Mvp.Platforms;

    public static class HttpPresenterBinderFactory
    {
        public static HttpPresenterBinder Create(
            IHttpHandler httpHandler,
            HttpContext context)
        {
            return Create(
                new[] { httpHandler },
                context,
                PlatformServices.Current,
                MessageCoordinatorBlackhole_.Instance);
        }

        public static HttpPresenterBinder Create(
            IEnumerable<Control> controls,
            HttpContext context)
        {
            var messageBus = PlatformServices.Current.MessageBusFactory.Create();
            var messageCoordinator = messageBus as IMessageCoordinator;
            if (messageCoordinator == null) {
                throw new NotSupportedException(
                    "The HTTP presenter binder requires the message bus to implement IMessageCoordinator.");
            }

            return Create(
                controls,
                context,
                PlatformServices.Current,
                messageCoordinator);
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

        class MessageCoordinatorBlackhole_ : IMessageCoordinator
        {
            static readonly IMessageCoordinator Instance_ = new MessageCoordinatorBlackhole_();

            public static IMessageCoordinator Instance { get { return Instance_; } }

            public void Publish<T>(T message)
            {
                throw new NotSupportedException();
            }

            public void Subscribe<T>(Action<T> onNext)
            {
                throw new NotSupportedException();
            }

            public void Dispose() { }
        }
    }
}
