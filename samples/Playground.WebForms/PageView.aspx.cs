namespace Playground.WebForms
{
    using System;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Presenters;
    using Playground.WebForms.Views.Models;

    [PresenterBinding(typeof(HelloWorldPresenter))]
    public partial class PageView : MvpPage<HelloWorldModel>
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            DataBind();

            base.OnPreRenderComplete(e);
        }
    }
}