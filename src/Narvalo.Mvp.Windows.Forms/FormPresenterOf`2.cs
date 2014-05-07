// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using Narvalo.Mvp;

    public abstract class FormPresenterOf<TView, TViewModel>
        : PresenterOf<TView, TViewModel>, IFormPresenter<TView>
        where TView : class, IView<TViewModel>
        where TViewModel : class, new()
    {
        protected FormPresenterOf(TView view) : base(view) { }

        public abstract void OnBindingComplete();
    }
}
