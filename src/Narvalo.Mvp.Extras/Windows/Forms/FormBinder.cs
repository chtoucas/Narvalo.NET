// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.Windows.Forms;
    using Narvalo;
    using Narvalo.Mvp.PresenterBinding;

    internal sealed class FormBinder
    {
        readonly PresenterBinder _presenterBinder;

        public FormBinder(Form form)
        {
            DebugCheck.NotNull(form);

            _presenterBinder = new PresenterBinder(this);
            _presenterBinder.PresenterCreated += (sender, e) =>
            {
                var presenter = e.Presenter as IFormPresenter;
                if (presenter != null) {
                    presenter.OnBindingComplete();
                }
            };

            form.Load += (sender, e) => _presenterBinder.PerformBinding();
            form.Disposed += (sender, e) => _presenterBinder.Release();
        }

        public void RegisterView(IView view)
        {
            DebugCheck.NotNull(view);

            _presenterBinder.RegisterView(view);
        }
    }
}
