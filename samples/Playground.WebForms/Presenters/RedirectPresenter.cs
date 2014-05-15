namespace Playground.WebForms.Presenters
{
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views;

    public class RedirectPresenter : HttpPresenter<IRedirectView>
    {
        public RedirectPresenter(IRedirectView view)
            : base(view)
        {
            View.ActionAccepted += (sender, e) => HttpContext.Response.Redirect("~/RedirectTo.aspx");
        }
    }
}