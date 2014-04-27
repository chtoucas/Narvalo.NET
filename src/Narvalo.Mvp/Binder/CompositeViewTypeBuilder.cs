// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using Narvalo;

    public sealed class CompositeViewTypeBuilder
    {
        static readonly CompositeViewTypeBuilder Instance_
            = new CompositeViewTypeBuilder();

        readonly Lazy<ICompositeViewTypeFactory> _factory;

        Func<ICompositeViewTypeFactory> _factoryThunk;

        CompositeViewTypeBuilder() : this(() => new DefaultCompositeViewTypeFactory()) { }

        CompositeViewTypeBuilder(Func<ICompositeViewTypeFactory> factoryThunk)
        {
            _factoryThunk = factoryThunk;
            _factory = new Lazy<ICompositeViewTypeFactory>(() => _factoryThunk());
        }

        public static CompositeViewTypeBuilder Current { get { return Instance_; } }

        public ICompositeViewTypeFactory Factory { get { return _factory.Value; } }

        public void SetFactory(ICompositeViewTypeFactory factory)
        {
            Require.NotNull(factory, "factory");

            _factoryThunk = () => factory;
        }
    }
}
