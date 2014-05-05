// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    public abstract class FormPresenter<TView> : IFormPresenter<TView> 
        where TView : class, IView
    {
        readonly TView _view;

        protected FormPresenter(TView view)
        {
            Require.NotNull(view, "view");

            _view = view;
        }

        public IMessageBus Messages { get; set; }

        public TView View { get { return _view; } }

        public abstract void OnBindingComplete();
    }
}
