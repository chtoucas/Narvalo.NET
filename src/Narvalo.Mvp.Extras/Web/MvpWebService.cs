// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using System.Web.Services;
    using Narvalo.Mvp.PresenterBinding;

    public abstract class MvpWebService : WebService, IView
    {
        readonly HttpPresenterBinder _presenterBinder;
        readonly bool _throwIfNoPresenterBound;

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

        protected void ReleaseView()
        {
            // REVIEW: When is it called? Overrides Dispose?
            _presenterBinder.Release();
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
