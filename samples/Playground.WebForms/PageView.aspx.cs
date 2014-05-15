namespace Playground.WebForms
{
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Presenters;
    using Playground.WebForms.Views.Models;

    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class PageViewPage : MvpPage<StringModel> { }
}