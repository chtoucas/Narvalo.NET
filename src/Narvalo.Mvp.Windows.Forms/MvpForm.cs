// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System.ComponentModel;
    using System.Windows.Forms;

    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Windows.Forms.Internal;

    // Cf. https://msdn.microsoft.com/library/86faxx0d%28v=vs.110%29.aspx (Windows Forms)

    public partial class MvpForm : Form, IView
    {
        private readonly bool _throwIfNoPresenterBound;

        private bool _disposed = false;
        private PresenterBinder _presenterBinder;

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

        protected override void OnControlAdded(ControlEventArgs e)
        {
            Require.NotNull(e, "e");

            var view = e.Control as IView;

            if (!DesignMode && view != null) {
                _presenterBinder.RegisterView(view);
            }

            base.OnControlAdded(e);
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
