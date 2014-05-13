// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class HttpPresenterBinder : PresenterBinder
    {
        readonly HttpContext _context;
        readonly IMessageCoordinator _messageCoordinator;

        public HttpPresenterBinder(
            IEnumerable<object> hosts,
            HttpContext context,
            IPresenterDiscoveryStrategy presenterDiscoveryStrategy,
            IPresenterFactory presenterFactory,
            ICompositeViewFactory compositeViewFactory,
            IMessageBusFactory messageBusFactory)
            : base(
                hosts,
                presenterDiscoveryStrategy,
                presenterFactory,
                compositeViewFactory,
                messageBusFactory)
        {
            Require.NotNull(context, "context");

            if (messageBusFactory.IsStatic) {
                throw new ArgumentException(
                    "The HTTP presenter binder requires a factory that creates transient message bus.",
                    "messageBusFactory");
            }

            _context = context;

            var messageCoordinator = MessageBus as IMessageCoordinator;
            if (messageCoordinator == null) {
                throw new NotSupportedException(
                    "The HTTP presenter binder requires the message bus to implement IMessageCoordinator.");
            }

            _messageCoordinator = messageCoordinator;
        }

        public IMessageCoordinator MessageCoordinator { get { return _messageCoordinator; } }

        public override void Release()
        {
            MessageCoordinator.Close();

            base.Release();
        }

        protected override void OnPresenterCreated(PresenterCreatedEventArgs args)
        {
            var presenter = args.Presenter as Internal.IHttpPresenter;
            if (presenter != null) {
                presenter.HttpContext = new HttpContextWrapper(_context);
            }

            base.OnPresenterCreated(args);
        }
    }
}
