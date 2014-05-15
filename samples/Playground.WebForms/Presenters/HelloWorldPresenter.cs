namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views.Models;

    public class HelloWorldPresenter : HttpPresenterOf<StringModel>
    {
        public HelloWorldPresenter(IView<StringModel> view)
            : base(view)
        {
            View.Model.Message = "If you see this message, something went wrong.";

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