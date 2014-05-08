// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Narvalo;
    using Narvalo.Mvp.Services;

    public class PresenterBinder
    {
        readonly IList<IPresenter> _presenters = new List<IPresenter>();
        readonly IList<IView> _viewsToBind = new List<IView>();

        readonly IEnumerable<object> _hosts;

        readonly ICompositeViewFactory _compositeViewFactory;
        readonly IPresenterDiscoveryStrategy _presenterDiscoveryStrategy;
        readonly IPresenterFactory _presenterFactory;

        readonly IMessageBus _messageBus;

        bool _bindingCompleted = false;

        public PresenterBinder(object host)
            : this(new[] { host }, new GlobalServicesContainer()) { }

        public PresenterBinder(IEnumerable<object> hosts)
            : this(hosts, new GlobalServicesContainer()) { }

        public PresenterBinder(
            IEnumerable<object> hosts,
            IPresenterDiscoveryStrategy presenterDiscoveryStrategy,
            IPresenterFactory presenterFactory,
            ICompositeViewFactory compositeViewFactory,
            IMessageBusFactory messageBusFactory)
        {
            Require.NotNull(hosts, "hosts");
            Require.NotNull(presenterDiscoveryStrategy, "presenterDiscoveryStrategy");
            Require.NotNull(presenterFactory, "presenterFactory");
            Require.NotNull(compositeViewFactory, "compositeViewFactory");
            Require.NotNull(messageBusFactory, "messageBusFactory");

            _hosts = hosts;
            _presenterDiscoveryStrategy = presenterDiscoveryStrategy;
            _presenterFactory = presenterFactory;
            _compositeViewFactory = compositeViewFactory;

            _messageBus = messageBusFactory.Create();

            foreach (var selfHostedView in hosts.OfType<IView>()) {
                RegisterView(selfHostedView);
            }
        }

        internal PresenterBinder(IEnumerable<object> hosts, IServicesContainer container)
            : this(
                hosts,
                container.PresenterDiscoveryStrategy,
                container.PresenterFactory,
                container.CompositeViewFactory,
                container.MessageBusFactory) { }

        public event EventHandler<PresenterCreatedEventArgs> PresenterCreated;

        public IMessageBus Messages { get { return _messageBus; } }

        public void PerformBinding()
        {
            try {
                if (_viewsToBind.Any()) {
                    var presenters = from binding in FindBindings_(_hosts)
                                     from view in GetViews_(binding)
                                     select CreatePresenter(binding, view);

                    foreach (var presenter in presenters) {
                        _presenters.Add(presenter);
                    }

                    _viewsToBind.Clear();
                }
            }
            finally {
                _bindingCompleted = true;
            }
        }

        public void RegisterView(IView view)
        {
            Require.NotNull(view, "view");

            _viewsToBind.Add(view);

            if (_bindingCompleted) {
                PerformBinding();
            }
        }

        public void Release()
        {
            var disposableMessageBus = _messageBus as IDisposable;
            if (disposableMessageBus != null) {
                disposableMessageBus.Dispose();
            }

            lock (_presenters) {
                foreach (var presenter in _presenters) {
                    _presenterFactory.Release(presenter);
                }

                _presenters.Clear();
            }
        }

        protected virtual IPresenter CreatePresenter(PresenterBindingParameter binding, IView view)
        {
            var presenter = _presenterFactory.Create(binding.PresenterType, binding.ViewType, view);

            presenter.Messages = _messageBus;

            OnPresenterCreated(new PresenterCreatedEventArgs(presenter));

            return presenter;
        }

        protected virtual void OnPresenterCreated(PresenterCreatedEventArgs args)
        {
            EventHandler<PresenterCreatedEventArgs> localHandler = PresenterCreated;

            if (localHandler != null) {
                localHandler(this, args);
            }
        }

        IEnumerable<PresenterBindingParameter> FindBindings_(IEnumerable<Object> hosts)
        {
            var viewsToBind = _viewsToBind.Distinct();

            var result = _presenterDiscoveryStrategy.FindBindings(hosts, viewsToBind);

            var unboundViews = from _ in viewsToBind.Except(result.BoundViews)
                               where _.ThrowIfNoPresenterBound
                               select _;

            if (unboundViews.Any()) {
                throw new PresenterBindingException(String.Format(
                    CultureInfo.InvariantCulture,
                    @"Failed to find presenter for view of type {0}.",
                    unboundViews.First().GetType().FullName
                ));
            }

            return result.Bindings;
        }

        IEnumerable<IView> GetViews_(PresenterBindingParameter binding)
        {
            IEnumerable<IView> views;

            switch (binding.BindingMode) {
                case PresenterBindingMode.Default:
                    views = binding.Views;
                    break;

                case PresenterBindingMode.SharedPresenter:
                    views = new[]
                    {
                        _compositeViewFactory.Create(binding.ViewType, binding.Views)
                    };
                    break;

                default:
                    throw new PresenterBindingException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported.",
                        binding.BindingMode));
            }

            return views;
        }
    }
}
