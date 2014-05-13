// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.ComponentModel;
    using System.Windows.Forms;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Windows.Forms.Core;

    public partial class MvpUserControl : UserControl, IView
    {
        readonly bool _throwIfNoPresenterBound;

        bool _disposed = false;
        PresenterBinder _presenterBinder;

        protected MvpUserControl() : this(true) { }

        protected MvpUserControl(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
                // Remark: We can not use the "DesignMode" property in the constructor.
                // See http://stackoverflow.com/questions/1774689/how-to-have-code-in-the-constructor-that-will-not-be-executed-at-design-time-by

                _presenterBinder = FormsPresenterBinderFactory.Create(this);
                _presenterBinder.PerformBinding();
            }
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    _presenterBinder.Release();
                    _presenterBinder = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        //// REVIEW: Use a different execution point in the windows forms lifecycle.
        //protected override void OnCreateControl()
        //{
        //    if (!DesignMode) {
        //        var form = FindForm();

        //        if (form == null) {
        //            throw new InvalidOperationException(
        //                "Controls can only be registered once they have been added to the live control tree.");
        //        }

        //        FormHost.Register(form).RegisterView(this);
        //    }

        //    base.OnCreateControl();
        //}
    }
}
