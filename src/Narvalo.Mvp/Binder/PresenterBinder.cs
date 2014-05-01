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
        static readonly ICompositeViewFactory CompositeViewFactory_
            = BindingServicesContainer.Current.CompositeViewFactory;
        static readonly IPresenterDiscoveryStrategy PresenterDiscoveryStrategy_
            = BindingServicesContainer.Current.PresenterDiscoveryStrategy;
        static readonly IPresenterFactory PresenterFactory_
            = BindingServicesContainer.Current.PresenterFactory;

        static readonly IMessageBus MessageBus_ = MessageBusContainer.Current.Value;

        readonly IList<IPresenter> _presenters = new List<IPresenter>();
        readonly IList<IView> _viewsToBind = new List<IView>();

        readonly IEnumerable<object> _hosts;

        bool _bindingCompleted = false;

        public PresenterBinder(object host) : this(new[] { host }) { }

        public PresenterBinder(IEnumerable<object> hosts)
        {
            Require.NotNull(hosts, "hosts");

            _hosts = hosts;

            foreach (var selfHostedView in hosts.OfType<IView>()) {
                RegisterView(selfHostedView);
            }
        }

        public event EventHandler<PresenterCreatedEventArgs> PresenterCreated;

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
            MessageBus_.Close();

            lock (_presenters) {
                foreach (var presenter in _presenters) {
                    PresenterFactory_.Release(presenter);
                }

                _presenters.Clear();
            }
        }

        static IEnumerable<IView> GetViews_(PresenterBinding binding)
        {
            IEnumerable<IView> views;

            switch (binding.BindingMode) {
                case PresenterBindingMode.Default:
                    views = binding.Views;
                    break;

                case PresenterBindingMode.SharedPresenter:
                    views = new[]
                    {
                        CompositeViewFactory_.Create(binding.ViewType, binding.Views)
                    };
                    break;

                default:
                    throw new BindingException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported by this method.",
                        binding.BindingMode));
            }

            return views;
        }

        void OnPresenterCreated_(PresenterCreatedEventArgs args)
        {
            EventHandler<PresenterCreatedEventArgs> localHandler = PresenterCreated;

            if (localHandler != null) {
                localHandler(this, args);
            }
        }

        IPresenter CreatePresenter_(PresenterBinding binding, IView view)
        {
            var presenter = PresenterFactory_.Create(binding.PresenterType, binding.ViewType, view);

            // TODO: On the way to remove MessageBus from PresenterBinder.
            presenter.Messages = MessageBus_;

            OnPresenterCreated_(new PresenterCreatedEventArgs(presenter));

            return presenter;
        }

        IEnumerable<PresenterBinding> FindBindings_(IEnumerable<Object> hosts)
        {
            var results = PresenterDiscoveryStrategy_.FindBindings(hosts, _viewsToBind.Distinct());

            // REVIEW: There is something fishy here...
            var unboundViews = from result in results
                               from view in result.Views
                               where result.Bindings.IsEmpty()
                               select view;

            if (unboundViews.Any()) {
                throw new BindingException(String.Format(
                    CultureInfo.InvariantCulture,
                    @"Failed to find presenter for view instance of type {0}.",
                    unboundViews.First().GetType().FullName
                ));
            }

            return from result in results
                   from binding in result.Bindings
                   select binding;
        }
    }
}
