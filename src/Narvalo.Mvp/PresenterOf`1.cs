// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public abstract class PresenterOf<TViewModel> : IPresenter<IView<TViewModel>>
        where TViewModel: class, new()
    {
        readonly IView<TViewModel> _view;

        protected PresenterOf(IView<TViewModel> view)
        {
            Require.NotNull(view, "view");

            _view = view;
            _view.Model = new TViewModel();
        }

        public IView<TViewModel> View { get { return _view; } }
    }
}
