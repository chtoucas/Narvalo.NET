// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System;
    using System.Windows.Forms;

    public partial class MvpForm : Form, IView
    {
        readonly bool _throwIfNoPresenterBound;

        protected MvpForm() : this(true) { }

        protected MvpForm(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        // REVIEW: Use a different execution point in the windows forms lifecycle.
        // NB: During construction, this conflicts with the design-mode in Visual Studio
        protected override void OnCreateControl()
        {
            // See http://stackoverflow.com/questions/1774689/how-to-have-code-in-the-constructor-that-will-not-be-executed-at-design-time-by
            if (!DesignMode) {
                FormHost.RegisterForm(this);
            }

            base.OnCreateControl();
        }
    }
}
