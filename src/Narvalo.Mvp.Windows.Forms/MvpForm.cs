// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.ComponentModel;
    using System.Windows.Forms;
    using Narvalo.Mvp.PresenterBinding;

    public partial class MvpForm : Form, IView
    {
        readonly bool _throwIfNoPresenterBound;

        bool _disposed = false;
        PresenterBinder _presenterBinder;

        protected MvpForm() : this(true) { }

        protected MvpForm(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime) {
                // Remark: We can not use the "DesignMode" property in the constructor.
                // See http://stackoverflow.com/questions/1774689/how-to-have-code-in-the-constructor-that-will-not-be-executed-at-design-time-by

                _presenterBinder = PresenterBinderFactory.Create(this);
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
    }
}
