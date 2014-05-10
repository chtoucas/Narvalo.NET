// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Collections.Generic;
    using Narvalo;
    using Narvalo.Mvp.Platforms;

    public static class PresenterBinderFactory
    {
        public static PresenterBinder Create(
            IEnumerable<object> hosts,
            IPlatformServices container)
        {
            Require.NotNull(container, "container");

            return new PresenterBinder(
                hosts,
                container.PresenterDiscoveryStrategy,
                container.PresenterFactory,
                container.CompositeViewFactory);
        }
    }
}
