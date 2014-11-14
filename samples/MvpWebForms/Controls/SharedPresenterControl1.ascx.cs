namespace MvpWebForms.Controls
{
    using Narvalo.Mvp;
    using Narvalo.Web.Mvp;
    using MvpWebForms.Presenters;
    using MvpWebForms.Views;

    [PresenterBinding(
        typeof(SharedPresenter),
        ViewType = typeof(IView<StringModel>),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class SharedPresenterControl1 : MvpUserControl<StringModel> { }
}