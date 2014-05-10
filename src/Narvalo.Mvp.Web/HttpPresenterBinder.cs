// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
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
            IMessageCoordinatorFactory messageCoordinatorFactory)
            : base(
                hosts,
                presenterDiscoveryStrategy,
                presenterFactory,
                compositeViewFactory)
        {
            Require.NotNull(context, "context");
            Require.NotNull(messageCoordinatorFactory, "messageCoordinatorFactory");

            _context = context;
            _messageCoordinator = messageCoordinatorFactory.Create();
        }

        public IMessageCoordinator MessageCoordinator { get { return _messageCoordinator; } }

        public override void Release()
        {
            var disposableMessageCoordinator = _messageCoordinator as IDisposable;
            if (disposableMessageCoordinator != null) {
                disposableMessageCoordinator.Dispose();
            }

            base.Release();
        }

        protected override void OnPresenterCreated(PresenterCreatedEventArgs args)
        {
            var presenter = args.Presenter as Internal.IHttpPresenter;
            if (presenter != null) {
                presenter.HttpContext = new HttpContextWrapper(_context);
                presenter.Messages = _messageCoordinator;
            }

            base.OnPresenterCreated(args);
        }
    }
}
