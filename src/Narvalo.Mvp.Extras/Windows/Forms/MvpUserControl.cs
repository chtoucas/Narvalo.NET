// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.Windows.Forms;

    public partial class MvpUserControl : UserControl, IView
    {
        protected override void OnCreateControl()
        {
            if (!DesignMode) {
                FormViewHost.Register(this);
            }

            base.OnCreateControl();
        }
    }
}
