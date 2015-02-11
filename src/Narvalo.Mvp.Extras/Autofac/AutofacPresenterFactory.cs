// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Autofac
{
    using System;
    using System.Collections.Generic;
    using global::Autofac;
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
            // and then resolve the presenter, but it won't work.
            //var presenterScope = _container.BeginLifetimeScope(
            //    _ => _.RegisterInstance(view).As(viewType));
            //var presenter = (IPresenter)presenterScope.Resolve(presenterType);

            // NB: Another option is to not register the presenters using the 
            // ContainerBuilder and rather do it dynamically:
            //var presenterScope = _container.BeginLifetimeScope(_ => _.RegisterType(presenterType))
            var presenterScope = _container.BeginLifetimeScope();

            var presenter = (IPresenter)presenterScope.Resolve(
                presenterType,
                new TypedParameter(viewType, view));

            lock (Lock_) {
                _lifetimeScopes[presenter.GetHashCode()] = presenterScope;
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

        //class LooselyTypedParameter : ConstantParameter
        //{
        //    public LooselyTypedParameter(Type type, object value)
        //        : base(value, pi => pi.ParameterType.IsAssignableFrom(type))
        //    {
        //        Enforce.NotNull(type, "type");
        //    }
        //}
    }
}