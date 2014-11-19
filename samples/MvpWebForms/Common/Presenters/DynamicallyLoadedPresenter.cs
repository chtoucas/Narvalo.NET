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