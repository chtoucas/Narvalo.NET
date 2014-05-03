// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#define TEST

namespace Narvalo.Mvp
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Narvalo.Mvp.PresenterBinding;

#if TEST

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
            // REVIEW: I would prefer to register the view as a dependency and then resolve
            // the presenter.
            var innerScope = _container.BeginLifetimeScope();

            var presenter = (IPresenter)innerScope.Resolve(
                presenterType,
                new LooselyTypedParameter(viewType, view));

            // TODO: Inject IMessageBus.
            //presenter.Messages = innerScope.Resolve<IMessageBus>();

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

        public IPresenter Create(Type presenterType, Type viewType, IView view)
        {
            var lifetimeScope = _container.BeginLifetimeScope(builder =>
            {
                builder.RegisterType(presenterType).AsSelf();
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

            lifetimeScope.Dispose();
        }
    }

#endif
}