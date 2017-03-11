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

        public IEnumerable<IView> BoundViews => _boundViews;

        public IEnumerable<PresenterBindingParameter> Bindings => _bindings;
    }
}
