// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System;
    using System.Windows.Forms;
    using Narvalo.Mvp.Windows.Forms.Internal;

    public partial class MvpUserControl : UserControl, IView
    {
        readonly bool _throwIfNoPresenterBound;

        protected MvpUserControl() : this(true) { }

        protected MvpUserControl(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        // REVIEW: Use a different execution point in the windows forms lifecycle.
        protected override void OnCreateControl()
        {
            if (!DesignMode) {
                FormHost.RegisterControl(this);
            }

            base.OnCreateControl();
        }
    }
}
