namespace MvpWebForms.Presenters
{
    using Narvalo.Web.Mvp;
    using MvpWebForms.Views;

    public sealed class RedirectPresenter : HttpPresenter<IRedirectView>
    {
        public RedirectPresenter(IRedirectView view)
            : base(view)
        {
            View.ActionAccepted += (sender, e) => HttpContext.Response.Redirect("~/RedirectTo.aspx");
        }
    }
}