// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Autofac
{
    using System;
    using System.Collections.Generic;
    using global::Autofac;
    using global::Autofac.Core;
    using Narvalo.Mvp;
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
            // REVIEW: I would prefer to register the view as a dependency 
            // and then resolve the presenter.
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
            Require.NotNull(presenter, "presenter");

            var presenterKey = presenter.GetHashCode();

            var lifetimeScope = _lifetimeScopes[presenterKey];

            lock (Lock_) {
                _lifetimeScopes.Remove(presenterKey);
            }

            lifetimeScope.Dispose();
        }

        class LooselyTypedParameter : ConstantParameter
        {
            public LooselyTypedParameter(Type type, object value)
                : base(value, pi => pi.ParameterType.IsAssignableFrom(type))
            {
                Enforce.NotNull(type, "type");
            }
        }
    }
}