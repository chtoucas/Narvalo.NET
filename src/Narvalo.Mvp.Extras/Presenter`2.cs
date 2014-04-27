// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    public abstract class Presenter<TView, TModel> : IPresenter<TView>
        where TView : IView<TModel>
        where TModel : class, new()
    {
        readonly TView _view;

        protected Presenter(TView view)
        {
            view.Model = new TModel();

            _view = view;
        }

        public IMessageBus Messages { get; set; }

        public TView View { get { return _view; } }
    }
}
