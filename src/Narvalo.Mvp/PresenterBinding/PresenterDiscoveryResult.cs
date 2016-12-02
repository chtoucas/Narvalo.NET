// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Collections.Generic;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif

    using static System.Diagnostics.Contracts.Contract;

    public sealed class PresenterDiscoveryResult
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
                Ensures(Result<IEnumerable<IView>>() != null);

                return _boundViews;
            }
        }

        public IEnumerable<PresenterBindingParameter> Bindings
        {
            get
            {
                Ensures(Result<IEnumerable<PresenterBindingParameter>>() != null);

                return _bindings;
            }
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Invariant(_bindings != null);
            Invariant(_boundViews != null);
        }

#endif
    }
}