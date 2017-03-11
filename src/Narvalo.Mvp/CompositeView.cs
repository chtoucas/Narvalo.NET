// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Narvalo;
    using Narvalo.Mvp.Properties;

    // NB: Must stay public for "CompositeViewTypeBuilder" to work.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract partial class CompositeView<TView> : ICompositeView where TView : IView
    {
        private readonly IList<TView> _views = new List<TView>();

        public abstract event EventHandler Load;

        public bool ThrowIfNoPresenterBound => true;

        /// <summary>
        /// Gets the list of individual views represented by this composite view.
        /// </summary>
        protected internal IEnumerable<TView> Views => _views;

        /// <summary>
        /// Adds the specified view instance to the composite view collection.
        /// </summary>
        public void Add(IView view)
        {
            Require.NotNull(view, nameof(view));

            if (!(view is TView))
            {
                throw new ArgumentException(Format.Current(
                        Strings.CompositeView_TypeMismatch,
                        typeof(TView).FullName,
                        view.GetType().FullName),
                    nameof(view));
            }

            _views.Add((TView)view);
        }
    }
}
