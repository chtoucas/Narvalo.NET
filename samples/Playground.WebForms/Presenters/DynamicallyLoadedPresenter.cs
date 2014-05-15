namespace Playground.WebForms.Presenters
{
    using Narvalo.Mvp;
    using Playground.WebForms.Views;

    public class DynamicallyLoadedPresenter : Presenter<IDynamicallyLoadedView>
    {
        public DynamicallyLoadedPresenter(IDynamicallyLoadedView view)
            : base(view)
        {
            View.Load += (sender, e) => View.PresenterWasBound = true;
        }
    }
}