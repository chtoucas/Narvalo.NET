// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.Windows.Forms;
    using Narvalo.Mvp.PresenterBinding;

    public partial class MvpUserControl : UserControl, IView
    {
        bool _disposed = false;
        PresenterBinder _presenterBinder;

        protected override void OnCreateControl()
        {
            if (!DesignMode) {
                _presenterBinder = new PresenterBinder(this);
                _presenterBinder.PerformBinding();
            }

            base.OnCreateControl();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    if (_presenterBinder != null) {
                        _presenterBinder.Release();
                        _presenterBinder = null;
                    }
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
