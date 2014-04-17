// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using Narvalo;

    public sealed class PresenterDiscoveryStrategyBuilder
    {
        static readonly PresenterDiscoveryStrategyBuilder Instance_
            = new PresenterDiscoveryStrategyBuilder();

        readonly Lazy<IPresenterDiscoveryStrategy> _factory;

        Func<IPresenterDiscoveryStrategy> _factoryThunk;

        PresenterDiscoveryStrategyBuilder() : this(() => new AttributeBasedPresenterDiscoveryStrategy()) { }

        PresenterDiscoveryStrategyBuilder(Func<IPresenterDiscoveryStrategy> factoryThunk)
        {
            _factoryThunk = factoryThunk;
            _factory = new Lazy<IPresenterDiscoveryStrategy>(() => _factoryThunk());
        }

        public static PresenterDiscoveryStrategyBuilder Current { get { return Instance_; } }

        public IPresenterDiscoveryStrategy Factory { get { return _factory.Value; } }

        public void SetFactory(IPresenterDiscoveryStrategy factory)
        {
            Require.NotNull(factory, "factory");

            _factoryThunk = () => factory;
        }
    }
}
