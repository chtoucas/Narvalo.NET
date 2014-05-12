namespace Playground.WebForms
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Presenters;
    using Playground.WebForms.Views;
    using Playground.WebForms.Views.Models;

    [PresenterBinding(
        typeof(CompositeDemoPresenter),
        ViewType = typeof(ICompositeDemoView),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class CompositeViewPage : MvpPage<CompositeDemoViewModel>, ICompositeDemoView { }
}