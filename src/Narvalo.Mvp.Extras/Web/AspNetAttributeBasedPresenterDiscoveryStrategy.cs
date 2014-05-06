// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Collections.Generic;
    using Narvalo.Mvp.PresenterBinding;

    // TO BE REMOVED
    public sealed class AspNetAttributeBasedPresenterDiscoveryStrategy
        : IPresenterDiscoveryStrategy
    {
        readonly IPresenterDiscoveryStrategy _inner;

        public AspNetAttributeBasedPresenterDiscoveryStrategy()
        {
            _inner = new AttributeBasedPresenterDiscoveryStrategy(
                new AspNetPresenterBindingAttributesResolver());
        }

        public PresenterDiscoveryResult FindBindings(IEnumerable<object> hosts, IEnumerable<IView> views)
        {
            return _inner.FindBindings(hosts, views);
        }
    }
}
