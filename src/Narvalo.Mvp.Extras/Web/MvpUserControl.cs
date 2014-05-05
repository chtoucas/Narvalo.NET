// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web.UI;

    public abstract class MvpUserControl : UserControl, IView
    {
        protected MvpUserControl() { }

        protected override void OnInit(EventArgs e)
        {
            PageHost.RegisterControl(this, Context);

            base.OnInit(e);
        }
    }
}
