namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;

    public class TimePresenter : HttpPresenter<IView>
    {
        public TimePresenter(IView view)
            : base(view)
        {
            View.Load += (sender, e) => HttpContext.Response.Write("Current time is: " + DateTimeOffset.Now);
        }
    }
}