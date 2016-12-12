// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Collections.Generic;

    public sealed partial class PresenterDiscoveryResult
    {
        private readonly IEnumerable<IView> _boundViews;
        private readonly IEnumerable<PresenterBindingParameter> _bindings;

        public PresenterDiscoveryResult(
            IEnumerable<IView> boundViews,
            IEnumerable<PresenterBindingParameter> bindings)
        {
            Require.NotNull(boundViews, nameof(boundViews));
            Require.NotNull(bindings, nameof(bindings));

            _boundViews = boundViews;
            _bindings = bindings;
        }

        public IEnumerable<IView> BoundViews
        {
            get
            {
                Warrant.NotNull<IEnumerable<IView>>();

                return _boundViews;
            }
        }

        public IEnumerable<PresenterBindingParameter> Bindings
        {
            get
            {
                Warrant.NotNull<IEnumerable<PresenterBindingParameter>>();

                return _bindings;
            }
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Diagnostics.Contracts;

    public sealed partial class PresenterDiscoveryResult
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_bindings != null);
            Contract.Invariant(_boundViews != null);
        }
    }
}

#endif
