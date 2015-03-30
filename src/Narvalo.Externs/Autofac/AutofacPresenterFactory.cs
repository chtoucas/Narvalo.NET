// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Externs.Autofac
{
    using System;
    using System.Collections.Generic;

    using global::Autofac;
    using Narvalo.Mvp;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class AutofacPresenterFactory : IPresenterFactory
    {
        private static readonly Object s_Lock = new Object();

        private readonly IDictionary<int, ILifetimeScope> _lifetimeScopes
           = new Dictionary<int, ILifetimeScope>();

        private readonly IContainer _container;

        public AutofacPresenterFactory(IContainer container)
        {
            Require.NotNull(container, "container");

            _container = container;
        }

        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            // REVIEW: I would prefer to register the view as a dependency 
            // and then resolve the presenter, but it doesn't work 
            // in some situations.
            // var presenterScope = _container.BeginLifetimeScope(
            //    _ => _.RegisterInstance(view).As(viewType));
            // var presenter = (IPresenter)presenterScope.Resolve(presenterType);
            // NB: Another option is to not register the presenters using the 
            // ContainerBuilder and rather do it dynamically:
            // var presenterScope = _container.BeginLifetimeScope(_ => _.RegisterType(presenterType))
            var presenterScope = _container.BeginLifetimeScope();

            var presenter = (IPresenter)presenterScope.Resolve(
                presenterType,
                new TypedParameter(viewType, view));

            lock (s_Lock) {
                _lifetimeScopes[presenter.GetHashCode()] = presenterScope;
            }

            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            Require.NotNull(presenter, "presenter");

            var presenterKey = presenter.GetHashCode();

            var lifetimeScope = _lifetimeScopes[presenterKey];

            lock (s_Lock) {
                _lifetimeScopes.Remove(presenterKey);
            }

            lifetimeScope.Dispose();
        }

        ////class LooselyTypedParameter : ConstantParameter
        ////{
        ////    public LooselyTypedParameter(Type type, object value)
        ////        : base(value, pi => pi.ParameterType.IsAssignableFrom(type))
        ////    {
        ////        Enforce.NotNull(type, "type");
        ////    }
        ////}
    }
}