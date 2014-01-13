namespace Narvalo.Presentation.Mvp.Simple
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class PresenterBinder
    {
        static IPresenterDiscoveryStrategy DiscoveryStrategy_;
        static IPresenterFactory Factory_;

        readonly IList<IPresenter> _presenters = new List<IPresenter>();
        readonly IList<IView> _viewsRequiringBinding = new List<IView>();

        bool _initialBindingHasBeenPerformed;

        #region Constructeurs

        internal PresenterBinder(IView view) : this(new[] { view }) { }

        internal PresenterBinder(IEnumerable<IView> views)
        {
            foreach (var view in views) {
                RegisterView(view);
            }
        }

        #endregion

        /// <summary>
        /// Occurs when the binder creates a new presenter instance. Useful for
        /// populating extra information into presenters.
        /// </summary>
        public event EventHandler<PresenterCreatedEventArgs> PresenterCreated;

        ///<summary>
        /// Gets or sets the factory that the binder will use to create
        /// new presenter instances. This is pre-initialized to a
        /// default implementation but can be overriden if desired.
        /// This property can only be set once.
        ///</summary>
        ///<exception cref="ArgumentNullException">Thrown if a null value is passed to the setter.</exception>
        ///<exception cref="InvalidOperationException">Thrown if the property is being set for a second time.</exception>
        public static IPresenterFactory Factory
        {
            get
            {
                return Factory_ ?? (Factory_ = new DefaultPresenterFactory());
            }
            set
            {
                Requires.NotNull(value, "value");

                if (Factory_ != null) {
                    throw new InvalidOperationException(
                        Factory_ is DefaultPresenterFactory
                        ? "The factory has already been set, and can be not changed at a later time. In this case, it has been set to the default implementation. This happens if the factory is used before being explicitly set. If you wanted to supply your own factory, you need to do this in your Application_Start event."
                        : "You can only set your factory once, and should really do this in Application_Start.");
                }
                Factory_ = value;
            }
        }

        ///<summary>
        /// Gets or sets the strategy that the binder will use to discover which presenters should 
        /// be bound to which views.
        /// This is pre-initialized to a default implementation but can be overriden if desired. 
        /// To combine multiple
        /// strategies in a fallthrough approach, use <see cref="CompositePresenterDiscoveryStrategy"/>.
        ///</summary>
        ///<exception cref="ArgumentNullException">Thrown if a null value is passed to the setter.</exception>
        public static IPresenterDiscoveryStrategy DiscoveryStrategy
        {
            get
            {
                return DiscoveryStrategy_
                    ?? (DiscoveryStrategy_ = new AttributeBasedPresenterDiscoveryStrategy());
            }
            set
            {
                Requires.NotNull(value, "value");
                DiscoveryStrategy_ = value;
            }
        }

        /// <summary>
        /// Registers a view instance as being a candidate for binding. If
        /// <see cref="PerformBinding()"/> has not been called, the view will
        /// be queued until that time. If <see cref="PerformBinding()"/> has
        /// already been called, binding is attempted instantly.
        /// </summary>
        public void RegisterView(IView view)
        {
            Requires.NotNull(view, "view");

            _viewsRequiringBinding.Add(view);

            // If an initial binding has already been performed, go ahead
            // and bind this view straight away. This allows us to bind
            // dynamically created controls that are added after Page.Init.
            if (_initialBindingHasBeenPerformed) {
                PerformBinding();
            }
        }

        /// <summary>
        /// Attempts to bind any already registered views.
        /// </summary>
        public void PerformBinding()
        {
            try {
                if (_viewsRequiringBinding.Any()) {
                    var presenters = PerformBinding_(
                        _viewsRequiringBinding.Distinct(),
                        _ => OnPresenterCreated_(new PresenterCreatedEventArgs(_)));

                    foreach (var _ in presenters) {
                        _presenters.Add(_);
                    }

                    _viewsRequiringBinding.Clear();
                }
            }
            finally {
                _initialBindingHasBeenPerformed = true;
            }
        }

        /// <summary>
        /// Closes the message bus, releases each of the views from the
        /// presenters then releases each of the presenters from the factory
        /// (useful in IoC scenarios).
        /// </summary>
        public void Release()
        {
            lock (_presenters) {
                foreach (var presenter in _presenters) {
                    var presenter1 = presenter;

                    Factory_.Release(presenter);
                }
                _presenters.Clear();
            }
        }

        #region Membres privés

        void OnPresenterCreated_(PresenterCreatedEventArgs e)
        {
            EventHandler<PresenterCreatedEventArgs> localHandler = PresenterCreated;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

        static IEnumerable<IPresenter> PerformBinding_(
           IEnumerable<IView> candidates,
           Action<IPresenter> onCreated)
        {
            var bindings = GetBindings_(candidates);

            return BuildPresenters_(bindings, onCreated);
        }

        static IEnumerable<PresenterBinding> GetBindings_(IEnumerable<IView> candidates)
        {
            var results = DiscoveryStrategy.GetBindings(candidates);

            ThrowForViewsWithNoPresenterBound_(results);

            return results.SelectMany(r => r.Bindings);
        }

        static IEnumerable<IPresenter> BuildPresenters_(
           IEnumerable<PresenterBinding> bindings,
           Action<IPresenter> onCreated)
        {
            foreach (var _ in bindings) {
                yield return BuildPresenter_(_, onCreated);
            }
        }

        static IPresenter BuildPresenter_(
            PresenterBinding binding,
            Action<IPresenter> onCreated)
        {
            var presenter = Factory.Create(binding.PresenterType, binding.ViewType, binding.View);

            if (onCreated != null) {
                onCreated(presenter);
            }

            return presenter;
        }

        static void ThrowForViewsWithNoPresenterBound_(
            IEnumerable<PresenterDiscoveryResult> results)
        {
            var resultToThrowExceptionsFor = results
                .Where(_ => !_.Bindings.Any())
                .Where(_ => _
                    .Views
                    .Where(v => v.ThrowIfNoPresenterBound)
                    .Any())
                .FirstOrDefault();

            if (resultToThrowExceptionsFor == null)
                return;

            throw ExceptionFactory.InvalidOperation(
                @"Failed to find presenter for view instance of {0}. If you do not want this exception to be thrown, set ThrowExceptionIfNoPresenterBound to false on your view.",
                resultToThrowExceptionsFor
                    .Views
                    .Where(v => v.ThrowIfNoPresenterBound)
                    .First()
                    .GetType()
                    .FullName
            );
        }

        #endregion
    }
}