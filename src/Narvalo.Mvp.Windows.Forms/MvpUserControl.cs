// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.Windows.Forms;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Windows.Forms.Internal;

    public partial class MvpUserControl : UserControl, IView
    {
        readonly bool _throwIfNoPresenterBound;

        bool _disposed = false;
        PresenterBinder _presenterBinder;

        protected MvpUserControl() : this(true) { }

        protected MvpUserControl(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        protected override void OnCreateControl()
        {
            if (!DesignMode) {
                var form = FindForm();

                if (!(form is MvpForm)) {
                    _presenterBinder = PresenterBinderFactory.Create(this);
                    _presenterBinder.PerformBinding();
                }
            }

            base.OnCreateControl();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing && _presenterBinder != null) {
                    _presenterBinder.Release();
                    _presenterBinder = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
