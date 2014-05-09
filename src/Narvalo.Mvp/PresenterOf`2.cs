// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public abstract class PresenterOf<TView, TViewModel> : IPresenter<TView>
        where TView : class, IView<TViewModel>
        where TViewModel : class, new()
    {
        readonly TView _view;

        protected PresenterOf(TView view)
        {
            Require.NotNull(view, "view");

            _view = view;
            _view.Model = new TViewModel();
        }

        public TView View { get { return _view; } }
    }
}
