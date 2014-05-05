namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Views;

    public class DynamicallyLoadedPresenter : Presenter<IDynamicallyLoadedView>
    {
        public DynamicallyLoadedPresenter(IDynamicallyLoadedView view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.PresenterWasBound = true;
        }
    }
}