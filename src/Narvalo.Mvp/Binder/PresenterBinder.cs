// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Mvp.Internal;

    public sealed class PresenterBinder
    {
        readonly ICompositeViewTypeFactory _compositeViewTypeFactory
            = new CompositeViewTypeFactory();
        readonly IMessageBus _messages = new MessageBus();
        readonly IList<IPresenter> _presenters = new List<IPresenter>();
        readonly IList<IView> _viewsToBind = new List<IView>();

        readonly IPresenterDiscoveryStrategy _presenterDiscoveryStrategy;
        readonly IPresenterFactory _presenterFactory;

        readonly IEnumerable<object> _hosts;

        bool _bindingCompleted = false;

        public PresenterBinder(object host)
            : this(new[] { host }, null, null) { }

        public PresenterBinder(
            object host,
            IPresenterFactory presenterFactory,
            IPresenterDiscoveryStrategy presenterDiscoveryStrategy)
            : this(
                new[] { host },
                presenterFactory,
                presenterDiscoveryStrategy) { }

        public PresenterBinder(IEnumerable<object> hosts)
            : this(hosts, null, null) { }

        public PresenterBinder(
            IEnumerable<object> hosts,
            IPresenterFactory presenterFactory,
            IPresenterDiscoveryStrategy presenterDiscoveryStrategy)
        {
            Require.NotNull(hosts, "hosts");

            _hosts = hosts;
            _presenterFactory = presenterFactory ?? PresenterBuilder.Current.Factory;
            _presenterDiscoveryStrategy
                = presenterDiscoveryStrategy ?? PresenterDiscoveryStrategyBuilder.Current.Factory;

            foreach (var selfHostedView in hosts.OfType<IView>()) {
                RegisterView(selfHostedView);
            }
        }

        public event EventHandler<PresenterCreatedEventArgs> PresenterCreated;

        public IMessageBus Messages { get { return _messages; } }

        public void PerformBinding()
        {
            try {
                if (_viewsToBind.Any()) {
                    var presenters = from binding in FindBindings_(_hosts)
                                     from view in GetViews_(binding)
                                     select CreatePresenter_(binding, view);

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
            _messages.Close();

            lock (_presenters) {
                foreach (var presenter in _presenters) {
                    _presenterFactory.Release(presenter);
                }

                _presenters.Clear();
            }
        }

        void OnPresenterCreated_(PresenterCreatedEventArgs args)
        {
            EventHandler<PresenterCreatedEventArgs> localHandler = PresenterCreated;

            if (localHandler != null) {
                localHandler(this, args);
            }
        }

        IView CreateCompositeView_(Type viewType, IEnumerable<IView> childViews)
        {
            var compositeViewType = _compositeViewTypeFactory.CreateCompositeViewType(viewType);
            var view = (ICompositeView)Activator.CreateInstance(compositeViewType);

            foreach (var item in childViews) {
                view.Add(item);
            }

            return view;
        }

        IPresenter CreatePresenter_(PresenterBinding binding, IView view)
        {
            var presenter = _presenterFactory.Create(binding.PresenterType, binding.ViewType, view);
            presenter.Messages = _messages;

            OnPresenterCreated_(new PresenterCreatedEventArgs(presenter));

            return presenter;
        }

        IEnumerable<PresenterBinding> FindBindings_(IEnumerable<Object> hosts)
        {
            var results = _presenterDiscoveryStrategy.FindBindings(hosts, _viewsToBind.Distinct());

            var unboundViews = from result in results
                               from view in result.Views
                               where result.Bindings.IsEmpty()
                               select view;

            if (unboundViews.Any()) {
                throw new InvalidOperationException(String.Format(
                    CultureInfo.InvariantCulture,
                    @"Failed to find presenter for view instance of type {0}.",
                    unboundViews.First().GetType().FullName
                ));
            }

            return from result in results
                   from binding in result.Bindings
                   select binding;
        }

        IEnumerable<IView> GetViews_(PresenterBinding binding)
        {
            IEnumerable<IView> views;

            switch (binding.BindingMode) {
                case PresenterBindingMode.Default:
                    views = binding.Views;
                    break;

                case PresenterBindingMode.SharedPresenter:
                    views = new[]
                    {
                        CreateCompositeView_(binding.ViewType, binding.Views)
                    };
                    break;

                default:
                    throw new NotSupportedException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported by this method.",
                        binding.BindingMode));
            }

            return views;
        }
    }
}
