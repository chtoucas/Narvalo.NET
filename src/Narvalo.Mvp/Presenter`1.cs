// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public abstract partial class Presenter<TView> : IPresenter<TView>, Internal.IPresenter
        where TView : class, IView
    {
        private readonly TView _view;

        protected Presenter(TView view)
        {
            Require.NotNull(view, nameof(view));

            _view = view;
        }

        public IMessageCoordinator Messages { get; private set; }

        public TView View { get { return _view; } }

        IMessageCoordinator Internal.IPresenter.Messages
        {
            set { Messages = value; }
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp
{
    using System.Diagnostics.Contracts;

    public abstract partial class Presenter<TView>
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_view != null);
        }
    }
}

#endif
