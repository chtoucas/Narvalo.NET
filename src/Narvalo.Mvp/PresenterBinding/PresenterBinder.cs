// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo;
    using Narvalo.Mvp.Properties;

    public partial class PresenterBinder
    {
        private readonly IList<IPresenter> _presenters = new List<IPresenter>();
        private readonly IList<IView> _viewsToBind = new List<IView>();

        private readonly ICompositeViewFactory _compositeViewFactory;
        private readonly IEnumerable<object> _hosts;
        private readonly IMessageCoordinator _messageCoordinator;
        private readonly IPresenterDiscoveryStrategy _presenterDiscoveryStrategy;
        private readonly IPresenterFactory _presenterFactory;

        private bool _bindingCompleted = false;

        public PresenterBinder(
            IEnumerable<object> hosts,
            IPresenterDiscoveryStrategy presenterDiscoveryStrategy,
            IPresenterFactory presenterFactory,
            ICompositeViewFactory compositeViewFactory,
            IMessageCoordinator messageCoordinator)
        {
            Require.NotNull(hosts, nameof(hosts));
            Require.NotNull(presenterDiscoveryStrategy, nameof(presenterDiscoveryStrategy));
            Require.NotNull(presenterFactory, nameof(presenterFactory));
            Require.NotNull(compositeViewFactory, nameof(compositeViewFactory));
            Require.NotNull(messageCoordinator, nameof(messageCoordinator));

            _hosts = hosts;
            _presenterDiscoveryStrategy = presenterDiscoveryStrategy;
            _presenterFactory = presenterFactory;
            _compositeViewFactory = compositeViewFactory;
            _messageCoordinator = messageCoordinator;

            foreach (var selfHostedView in hosts.OfType<IView>())
            {
                RegisterView(selfHostedView);
            }
        }

        public event EventHandler<PresenterEventArgs> PresenterCreated;

        public IMessageCoordinator MessageCoordinator => _messageCoordinator;

        public void PerformBinding()
        {
            try
            {
                if (_viewsToBind.Any())
                {
                    var presenters = from binding in FindBindings(_hosts)
                                     from view in GetViews(binding)
                                     select CreatePresenter(binding, view);

                    foreach (var presenter in presenters)
                    {
                        if (presenter == null) { continue; }

                        _presenters.Add(presenter);
                    }

                    _viewsToBind.Clear();
                }
            }
            finally
            {
                _bindingCompleted = true;
            }
        }

        public void RegisterView(IView view)
        {
            Require.NotNull(view, nameof(view));

            _viewsToBind.Add(view);

            if (_bindingCompleted)
            {
                PerformBinding();
            }
        }

        public virtual void Release()
        {
            MessageCoordinator.Close();

            lock (_presenters)
            {
                foreach (var presenter in _presenters)
                {
                    if (presenter == null) { continue; }

                    _presenterFactory.Release(presenter);
                }

                _presenters.Clear();
            }
        }

        protected IPresenter CreatePresenter(PresenterBindingParameter binding, IView view)
        {
            Require.NotNull(binding, nameof(binding));

            var presenter = _presenterFactory.Create(binding.PresenterType, binding.ViewType, view);

            if (presenter == null) { return null; }

            // TODO: Explain this.
            ((Internal.IPresenter)presenter).Messages = _messageCoordinator;

            OnPresenterCreated(new PresenterEventArgs(presenter));

            return presenter;
        }

        protected virtual void OnPresenterCreated(PresenterEventArgs args)
            => PresenterCreated?.Invoke(this, args);

        private IEnumerable<PresenterBindingParameter> FindBindings(IEnumerable<Object> hosts)
        {
            var viewsToBind = _viewsToBind.Distinct();

            var result = _presenterDiscoveryStrategy.FindBindings(hosts, viewsToBind);

            var unboundViews = from _ in viewsToBind.Except(result.BoundViews)
                               where _.ThrowIfNoPresenterBound
                               select _;

            if (unboundViews.Any())
            {
                // At this point, we know for sure that there is an unbound view.
                var unboundView = unboundViews.First();

                throw new PresenterBindingException(Format.Current(
                   Strings.PresenterBinder_NoPresenterFoundForView,
                    unboundViews.First().GetType().FullName));
            }

            return result.Bindings;
        }

        [SuppressMessage("Microsoft.Contracts", "Requires-7-71", Justification = "[Intentionally] Requires unreachable but CCCheck still proves no case is forgotten.")]
        private IEnumerable<IView> GetViews(PresenterBindingParameter binding)
        {
            Debug.Assert(binding != null);

            IEnumerable<IView> views;

            switch (binding.BindingMode)
            {
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
                    throw new ControlFlowException();
            }

            return views;
        }
    }
}
