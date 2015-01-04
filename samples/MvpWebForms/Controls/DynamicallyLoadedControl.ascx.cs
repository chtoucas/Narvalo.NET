// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Controls
{
    using MvpWebForms.Views;
    using Narvalo.Mvp.Web;

    public partial class DynamicallyLoadedControl : MvpUserControl, IDynamicallyLoadedView
    {
        public DynamicallyLoadedControl()
        {
            PreRender += (sender, e) => { if (PresenterWasBound) { cph1.Visible = true; cph2.Visible = false; } };
        }

        public bool PresenterWasBound { get; set; }
    }
}
