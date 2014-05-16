namespace Playground
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.Presenters;
    using Playground.Views;

    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class PageViewPage : MvpPage<StringModel> { }
}