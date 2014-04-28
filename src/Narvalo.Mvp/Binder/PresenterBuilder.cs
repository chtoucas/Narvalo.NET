// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using Narvalo;

    public sealed class PresenterBuilder
    {
        static readonly PresenterBuilder Instance_ = new PresenterBuilder();

        readonly Lazy<IPresenterFactory> _factory;

        Func<IPresenterFactory> _factoryThunk;

        PresenterBuilder() : this(() => new DefaultPresenterFactory()) { }

        PresenterBuilder(Func<IPresenterFactory> factoryThunk)
        {
            _factoryThunk = factoryThunk;
            _factory = new Lazy<IPresenterFactory>(() => _factoryThunk());
        }

        public static PresenterBuilder Current { get { return Instance_; } }

        public IPresenterFactory Factory { get { return _factory.Value; } }

        public void SetFactory(IPresenterFactory factory)
        {
            Require.NotNull(factory, "factory");

            if (_factory.IsValueCreated) {
                throw new InvalidOperationException("You can only set a IPresenterFactory once.");
            }

            _factoryThunk = () => factory;
        }
    }
}
