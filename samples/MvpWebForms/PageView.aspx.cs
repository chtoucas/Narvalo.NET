namespace MvpWebForms
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using MvpWebForms.Presenters;
    using MvpWebForms.Views;

    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class PageViewPage : MvpPage<StringModel> { }
}