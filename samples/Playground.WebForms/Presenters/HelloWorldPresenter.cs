namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views.Models;

    public class HelloWorldPresenter : HttpPresenter<IView<HelloWorldModel>>
    {
        public HelloWorldPresenter(IView<HelloWorldModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Message = HttpContext.User.Identity.IsAuthenticated
                ? String.Format("Hello {0}!", HttpContext.User.Identity.Name)
                : "Hello World!";
        }
    }
}