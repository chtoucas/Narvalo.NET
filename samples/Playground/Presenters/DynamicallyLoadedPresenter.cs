﻿namespace Playground.Presenters
{
    using Narvalo.Mvp;
    using Playground.Views;

    public sealed class DynamicallyLoadedPresenter : Presenter<IDynamicallyLoadedView>
    {
        public DynamicallyLoadedPresenter(IDynamicallyLoadedView view)
            : base(view)
        {
            View.Load += (sender, e) => View.PresenterWasBound = true;
        }
    }
}