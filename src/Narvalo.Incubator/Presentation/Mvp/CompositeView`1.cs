namespace Narvalo.Presentation.Mvp
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    ///<summary>
    /// Provides a basic implementation of the <see cref="ICompositeView"/> contract.
    ///</summary>
    public abstract class CompositeView<TView> : ICompositeView
        where TView : class, IView
    {
        readonly ICollection<TView> _views = new List<TView>();

        /// <summary />
        public bool ThrowIfNoPresenterBound
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the list of individual views represented by this composite view.
        /// </summary>
        protected internal IEnumerable<TView> Views
        {
            get { return _views; }
        }

        /// <summary>
        /// Occurs at the discretion of the view. For <see cref="MvpUserControl"/>
        /// implementations (the most commonly used), this is fired duing the ASP.NET
        /// Load event.
        /// </summary>
        public abstract event EventHandler Load;

        /// <summary>
        /// Adds the specified view instance to the composite view collection.
        /// </summary>
        public void Add(IView view)
        {
            Requires.NotNull(view, "view");

            if (!(view is TView)) {
                throw new ArgumentException(String.Format(
                    CultureInfo.InvariantCulture,
                    "Expected a view of type {0} but {1} was supplied.",
                    typeof(TView).FullName,
                    view.GetType().FullName
                ));
            }

            _views.Add((TView)view);
        }
    }
}