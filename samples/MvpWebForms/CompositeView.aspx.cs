namespace MvpWebForms
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using MvpWebForms.Presenters;
    using MvpWebForms.Views;

    [PresenterBinding(
        typeof(CompositePresenter),
        ViewType = typeof(IView<StringModel>),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class CompositeViewPage : MvpPage<StringModel> { }
}