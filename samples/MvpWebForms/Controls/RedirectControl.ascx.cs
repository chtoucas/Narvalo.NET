// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Controls
{
    using System;
    using MvpWebForms.Views;
    using Narvalo.Mvp.Web;

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
