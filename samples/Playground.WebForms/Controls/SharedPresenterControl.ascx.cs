namespace Playground.WebForms.Controls
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Presenters;
    using Playground.WebForms.Views.Models;

    [PresenterBinding(
        typeof(SharedPresenter),
        ViewType = typeof(IView<SharedPresenterViewModel>),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class SharedPresenterControl : MvpUserControl<SharedPresenterViewModel>
    {
    }
}