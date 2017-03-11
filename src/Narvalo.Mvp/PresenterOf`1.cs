// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public abstract partial class PresenterOf<TViewModel>
        : IPresenter<IView<TViewModel>>, Internal.IPresenter
        where TViewModel : class, new()
    {
        private readonly IView<TViewModel> _view;

        protected PresenterOf(IView<TViewModel> view)
        {
            Require.NotNull(view, nameof(view));

            _view = view;
            _view.Model = new TViewModel();
        }

        public IMessageCoordinator Messages { get; private set; }

        public IView<TViewModel> View { get { return _view; } }

        IMessageCoordinator Internal.IPresenter.Messages
        {
            set { Messages = value; }
        }
    }
}
