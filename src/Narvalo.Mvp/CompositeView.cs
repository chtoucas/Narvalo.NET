// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif

    using Narvalo;
    using Narvalo.Mvp.Properties;

    using static System.Diagnostics.Contracts.Contract;

    // NB: Must stay public for "CompositeViewTypeBuilder" to work.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class CompositeView<TView> : ICompositeView where TView : IView
    {
        private readonly IList<TView> _views = new List<TView>();

        public abstract event EventHandler Load;

        public bool ThrowIfNoPresenterBound => true;

        /// <summary>
        /// Gets the list of individual views represented by this composite view.
        /// </summary>
        protected internal IEnumerable<TView> Views
        {
            get { Warrant.NotNull<IEnumerable<TView>>(); return _views; }
        }

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

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Invariant(_views != null);
        }

#endif
    }
}
