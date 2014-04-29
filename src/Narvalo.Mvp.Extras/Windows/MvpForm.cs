// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows
{
    using System;
    using System.Windows.Forms;
    using Narvalo.Mvp.Binder;

    public partial class MvpForm : Form, IView
    {
        //bool _disposed = false;
        //PresenterBinder _presenterBinder;

        protected override void OnLoad(EventArgs e)
        {
            // See http://stackoverflow.com/questions/1774689/how-to-have-code-in-the-constructor-that-will-not-be-executed-at-design-time-by
            if (!DesignMode) {
                // FIXME: Must use something similar to what can be found with webformsmvp
                // otherwise cross-presenter messenging won't work.
                //_presenterBinder = new PresenterBinder(this);
                //_presenterBinder.PerformBinding();
                FormViewHost.Register(this);
            }

            base.OnLoad(e);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (!_disposed) {
        //        if (disposing) {
        //            if (_presenterBinder != null) {
        //                _presenterBinder.Release();
        //                _presenterBinder = null;
        //            }
        //        }

        //        _disposed = true;
        //    }

        //    base.Dispose(disposing);
        //}
    }
}
