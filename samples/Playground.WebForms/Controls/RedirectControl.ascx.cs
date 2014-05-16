namespace Playground.Controls
{
    using System;
    using Narvalo.Mvp.Web;
    using Playground.Views;

    public partial class RedirectControl : MvpUserControl, IRedirectView
    {
        public event EventHandler ActionAccepted;

        protected void Button_Click(object sender, EventArgs e)
        {
            OnActionAccepted();
        }

        void OnActionAccepted()
        {
            if (ActionAccepted != null) {
                ActionAccepted(this, EventArgs.Empty);
            }
        }
    }
}