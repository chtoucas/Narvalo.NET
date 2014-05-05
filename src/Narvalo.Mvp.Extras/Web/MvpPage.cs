// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web.UI;

    public abstract class MvpPage : Page, IView
    {
        protected MvpPage() { }

        protected override void OnInit(EventArgs e)
        {
            PageHost.RegisterPage(this, Context);

            base.OnInit(e);
        }
    }
}
