// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Presenters
{
    using MvpWebForms.Views;
    using Narvalo.Mvp;

    public sealed class DynamicallyLoadedPresenter : Presenter<IDynamicallyLoadedView>
    {
        public DynamicallyLoadedPresenter(IDynamicallyLoadedView view)
            : base(view)
        {
            View.Load += (sender, e) => View.PresenterWasBound = true;
        }
    }
}
