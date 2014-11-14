namespace MvpWebForms
{
    using Narvalo.Mvp;
    using Narvalo.Web.Mvp;
    using MvpWebForms.Presenters;
    using MvpWebForms.Views;

    [PresenterBinding(
        typeof(CompositePresenter),
        ViewType = typeof(IView<StringModel>),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class CompositeViewPage : MvpPage<StringModel> { }
}