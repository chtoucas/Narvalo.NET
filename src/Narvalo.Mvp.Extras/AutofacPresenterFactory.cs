// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#define TEST

namespace Narvalo.Mvp
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Autofac.Core;
    using Narvalo.Mvp.Binder;

#if TEST

    public sealed class AutofacPresenterFactory : IPresenterFactory
    {
        readonly IDictionary<int, ILifetimeScope> _lifetimeScopes
           = new Dictionary<int, ILifetimeScope>();

        readonly object _lock = new Object();

        readonly IContainer _container;

        public AutofacPresenterFactory(IContainer container)
        {
            Require.NotNull(container, "container");

            _container = container;
        }

        public void SelfRegister()
        {
            PresenterBuilder.Current.SetFactory(new AutofacPresenterFactory(_container));
        }

        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            var innerScope = _container.BeginLifetimeScope();

            var presenter = (IPresenter)innerScope.Resolve(
                presenterType,
                new LooselyTypedParameter(viewType, view));

            lock (_lock) {
                _lifetimeScopes[presenter.GetHashCode()] = innerScope;
            }

            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            var presenterKey = presenter.GetHashCode();

            var lifetimeScope = _lifetimeScopes[presenterKey];

            lock (_lock) {
                _lifetimeScopes.Remove(presenterKey);
            }

            lifetimeScope.Dispose();
        }

        class LooselyTypedParameter : ConstantParameter
        {
            public LooselyTypedParameter(Type type, object value)
                : base(value, pi => pi.ParameterType.IsAssignableFrom(type))
            {
                Require.NotNull(type, "type");
            }
        }
    }

#else

    public sealed class AutofacPresenterFactory : IPresenterFactory
    {
        readonly IDictionary<IPresenter, ILifetimeScope> _presentersToLifetimeScopes
            = new Dictionary<IPresenter, ILifetimeScope>();

        readonly object _lock = new Object();

        readonly IContainer _container;

        public AutofacPresenterFactory(IContainer container)
        {
            Require.NotNull(container, "container");

            _container = container;
        }

        public void SelfRegister()
        {
            PresenterBuilder.Current.SetFactory(new AutofacPresenterFactory(_container));
        }

        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            var lifetimeScope = _container.BeginLifetimeScope(builder =>
            {
                // We dynamically register the presenter type.
                //builder.RegisterType(presenterType).AsSelf();
                // REVIEW: I don't think this is necessary.
                builder.RegisterInstance((object)view).As(viewType);
            });

            var presenter = (IPresenter)lifetimeScope.Resolve(
                presenterType,
                new LooselyTypedParameter(viewType, view));

            lock (_lock) {
                _presentersToLifetimeScopes[presenter] = lifetimeScope;
            }

            return presenter;
        }

        public void Release(IPresenter presenter)
        {
            var lifetimeScope = _presentersToLifetimeScopes[presenter];

            lock (_lock) {
                _presentersToLifetimeScopes.Remove(presenter);
            }

            // Disposing the container will dispose any of the components
            // created within its lifetime scope.
            lifetimeScope.Dispose();
        }

        class LooselyTypedParameter : ConstantParameter
        {
            public LooselyTypedParameter(Type type, object value)
                : base(value, pi => pi.ParameterType.IsAssignableFrom(type))
            {
                Require.NotNull(type, "type");
            }
        }
    }

#endif
}