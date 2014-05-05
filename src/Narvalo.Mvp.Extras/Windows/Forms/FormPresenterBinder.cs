// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.Windows.Forms;
    using Narvalo.Mvp.PresenterBinding;

    internal sealed class FormPresenterBinder
    {
        readonly PresenterBinder _presenterBinder;

        public FormPresenterBinder(Form form)
        {
            _presenterBinder = new PresenterBinder(form);
            _presenterBinder.PresenterCreated += (sender, e) =>
            {
                var presenter = e.Presenter as IFormPresenter;
                if (presenter != null) {
                    presenter.OnBindingComplete();
                }
            };
        }

        public void PerformBinding()
        {
            _presenterBinder.PerformBinding();
        }

        public void RegisterView(IView view)
        {
            _presenterBinder.RegisterView(view);
        }

        public void Release()
        {
            _presenterBinder.Release();
        }
    }
}
