// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public abstract class Presenter<TView, TViewModel> : IPresenter<TView>
        where TView : IView<TViewModel>
        where TViewModel : class, new()
    {
        readonly TView _view;

        protected Presenter(TView view)
        {
            view.Model = new TViewModel();

            _view = view;
        }

        public IMessageBus Messages { get; set; }

        public TView View { get { return _view; } }
    }
}
