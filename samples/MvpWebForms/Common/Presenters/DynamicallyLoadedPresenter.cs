namespace MvpWebForms.Presenters
{
    using Narvalo.Mvp;
    using MvpWebForms.Views;

    public sealed class DynamicallyLoadedPresenter : Presenter<IDynamicallyLoadedView>
    {
        public DynamicallyLoadedPresenter(IDynamicallyLoadedView view)
            : base(view)
        {
            View.Load += (sender, e) => View.PresenterWasBound = true;
        }
    }
}