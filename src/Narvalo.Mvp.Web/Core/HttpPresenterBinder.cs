// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System.Collections.Generic;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif
    using System.Web;

    using Narvalo.Mvp;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class HttpPresenterBinder : PresenterBinder
    {
        private readonly HttpContext _context;

        public HttpPresenterBinder(
            IEnumerable<object> hosts,
            HttpContext context,
            IPresenterDiscoveryStrategy presenterDiscoveryStrategy,
            IPresenterFactory presenterFactory,
            ICompositeViewFactory compositeViewFactory,
            IMessageCoordinator messageCoordinator)
            : base(
                hosts,
                presenterDiscoveryStrategy,
                presenterFactory,
                compositeViewFactory,
                messageCoordinator)
        {
            Require.NotNull(context, nameof(context));
            Expect.NotNull(hosts);
            Expect.NotNull(presenterDiscoveryStrategy);
            Expect.NotNull(presenterFactory);
            Expect.NotNull(compositeViewFactory);
            Expect.NotNull(messageCoordinator);

            _context = context;
        }

        protected override void OnPresenterCreated(PresenterEventArgs args)
        {
            Require.NotNull(args, nameof(args));

            var presenter = args.Presenter as Internal.IHttpPresenter;
            if (presenter != null)
            {
                presenter.HttpContext = new HttpContextWrapper(_context);
            }

            base.OnPresenterCreated(args);
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_context != null);
        }

#endif
    }
}
