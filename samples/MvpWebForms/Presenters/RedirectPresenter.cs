namespace MvpWebForms.Presenters
{
    using MvpWebForms.Views;
    using Narvalo.Mvp.Web;

    public sealed class RedirectPresenter : HttpPresenter<IRedirectView>
    {
        public RedirectPresenter(IRedirectView view)
            : base(view)
        {
            View.ActionAccepted += (sender, e) => HttpContext.Response.Redirect("~/RedirectTo.aspx");
        }
    }
}