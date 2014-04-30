// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using Narvalo.Mvp.Internal;

    public sealed class PresenterDiscoveryStrategyProvider : ServiceProvider<IPresenterDiscoveryStrategy>
    {
        static readonly PresenterDiscoveryStrategyProvider Instance_
            = new PresenterDiscoveryStrategyProvider();

        PresenterDiscoveryStrategyProvider() : base(() => new DefaultPresenterDiscoveryStrategy()) { }

        public static PresenterDiscoveryStrategyProvider Current { get { return Instance_; } }
    }
}
