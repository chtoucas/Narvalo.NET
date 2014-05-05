namespace Playground.WebForms.Controls
{
    using System;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views;

    public partial class RedirectControl : MvpUserControl, IRedirectView
    {
        public event EventHandler ActionAccepted;

        protected void Button_Click(object sender, EventArgs e)
        {
            OnActionAccepted();
        }

        void OnActionAccepted()
        {
            var localHandler = ActionAccepted;

            if (localHandler != null) {
                localHandler(this, EventArgs.Empty);
            }
        }
    }
}