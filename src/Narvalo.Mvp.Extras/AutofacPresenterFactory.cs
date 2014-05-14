// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class AutofacPresenterFactory : IPresenterFactory
    {
        static readonly Object Lock_ = new Object();

        readonly IDictionary<int, ILifetimeScope> _lifetimeScopes
           = new Dictionary<int, ILifetimeScope>();

        readonly IContainer _container;

        public AutofacPresenterFactory(IContainer container)
        {
            Require.NotNull(container, "container");

            _container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            // REVIEW: I would prefer to register the view as a dependency and then resolve the presenter.
            var innerScope = _container.BeginLifetimeScope();

            var presenter = (IPresenter)innerScope.Resolve(
                presenterType,
                new LooselyTypedParameter(viewType, view));

            lock (Lock_) {
                _lifetimeScopes[presenter.GetHashCode()] = innerScope;
            }

            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            var presenterKey = presenter.GetHashCode();

            var lifetimeScope = _lifetimeScopes[presenterKey];

            lock (Lock_) {
                _lifetimeScopes.Remove(presenterKey);
            }

            lifetimeScope.Dispose();
        }
    }
}