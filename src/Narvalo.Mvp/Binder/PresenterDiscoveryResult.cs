// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System.Collections.Generic;

    public sealed class PresenterDiscoveryResult
    {
        readonly IList<IView> _boundViews;
        readonly IList<PresenterBinding> _bindings;

        public PresenterDiscoveryResult(
            IList<IView> boundViews,
            IList<PresenterBinding> bindings)
        {
            _boundViews = boundViews;
            _bindings = bindings;
        }

        public IList<IView> BoundViews { get { return _boundViews; } }

        public IList<PresenterBinding> Bindings { get { return _bindings; } }
    }
}