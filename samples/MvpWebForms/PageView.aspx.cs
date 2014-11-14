namespace MvpWebForms
{
    using Narvalo.Mvp;
    using Narvalo.Web.Mvp;
    using MvpWebForms.Presenters;
    using MvpWebForms.Views;

    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class PageViewPage : MvpPage<StringModel> { }
}