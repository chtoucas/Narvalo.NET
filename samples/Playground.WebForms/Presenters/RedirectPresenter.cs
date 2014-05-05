namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views;

    public class RedirectPresenter : HttpPresenter<IRedirectView>
    {
        public RedirectPresenter(IRedirectView view)
            : base(view)
        {
            View.ActionAccepted += ActionAccepted;
        }

        void ActionAccepted(object sender, EventArgs e)
        {
            HttpContext.Response.Redirect("~/RedirectTo.aspx");
        }
    }
}