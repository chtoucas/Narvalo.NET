// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.Windows.Forms;

    public partial class MvpForm : Form, IView
    {
        // REVIEW: Use a different execution point in the windows forms lifecycle.
        protected override void OnCreateControl()
        {
            // See http://stackoverflow.com/questions/1774689/how-to-have-code-in-the-constructor-that-will-not-be-executed-at-design-time-by
            if (!DesignMode) {
                FormHost.Register(this);
            }

            base.OnCreateControl();
        }
    }
}
