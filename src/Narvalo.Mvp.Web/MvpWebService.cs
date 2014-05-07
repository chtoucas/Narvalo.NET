// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using System.Web.Services;
    using Narvalo.Mvp.Web.Internal;

    public abstract class MvpWebService : WebService, IView
    {
        readonly bool _throwIfNoPresenterBound;

        bool _disposed = false;
        HttpPresenterBinder _presenterBinder;

        protected MvpWebService() : this(true) { }

        protected MvpWebService(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
            _presenterBinder = new HttpPresenterBinder(this, HttpContext.Current);
            _presenterBinder.PerformBinding();
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        public event EventHandler Load;

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

        protected virtual void OnLoad()
        {
            var localHandler = Load;

            if (localHandler != null) {
                localHandler(this, EventArgs.Empty);
            }
        }
    }
}
