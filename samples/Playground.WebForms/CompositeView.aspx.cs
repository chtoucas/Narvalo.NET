namespace Playground.WebForms
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Presenters;
    using Playground.WebForms.Views;

    [PresenterBinding(typeof(CompositeDemoPresenter),
        ViewType = typeof(ICompositeDemoView),
        BindingMode = PresenterBindingMode.SharedPresenter)]
    public partial class CompositeView : MvpPage
    {
    }
}