namespace Playground.Controls
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.Presenters;
    using Playground.Views;

    [PresenterBinding(
        typeof(SharedPresenter),
        ViewType = typeof(IView<StringModel>),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class SharedPresenterControl1 : MvpUserControl<StringModel> { }
}