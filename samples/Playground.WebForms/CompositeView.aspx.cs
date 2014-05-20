namespace Playground
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.Presenters;
    using Playground.Views;

    [PresenterBinding(
        typeof(CompositePresenter),
        ViewType = typeof(IView<StringModel>),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class CompositeViewPage : MvpPage<StringModel> { }
}