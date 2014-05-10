// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using Narvalo.Mvp;

    public abstract class FormPresenterOf<TViewModel>
        : PresenterOf<TViewModel>, IFormPresenter<IView<TViewModel>>
        where TViewModel : class, new()
    {
        protected FormPresenterOf(IView<TViewModel> view) : base(view) { }

        public IMessageBus Messages { get; set; }

        public abstract void OnBindingComplete();
    }
}
