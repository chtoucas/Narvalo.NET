// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows
{
    using System.Windows.Forms;
    using Narvalo.Mvp.Binder;

    // FIXME: Events do not work with shared presenter.

    public partial class MvpForm : Form, IView
    {
        bool _disposed = false;
        PresenterBinder _presenterBinder;

        protected override void OnCreateControl()
        {
            // See http://stackoverflow.com/questions/1774689/how-to-have-code-in-the-constructor-that-will-not-be-executed-at-design-time-by
            if (!DesignMode) {
                _presenterBinder = new PresenterBinder(this);
                _presenterBinder.PerformBinding();
                //FormViewHost.Register(this);
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
