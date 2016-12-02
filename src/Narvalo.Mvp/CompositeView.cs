// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;

    using Narvalo;

    // NB: Must stay public for "CompositeViewTypeBuilder" to work.
    [EditorBrowsableAttribute(EditorBrowsableState.Never)]
    public abstract class CompositeView<TView> : ICompositeView where TView : IView
    {
        private readonly IList<TView> _views = new List<TView>();

        public abstract event EventHandler Load;

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
        /// Adds the specified view instance to the composite view collection.
        /// </summary>
        public void Add(IView view)
        {
            Require.NotNull(view, nameof(view));

            if (!(view is TView))
            {
                throw new ArgumentException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        "Expected a view of type {0} but {1} was supplied.",
                        typeof(TView).FullName,
                        view.GetType().FullName),
                    "view");
            }

            _views.Add((TView)view);
        }
    }
}
