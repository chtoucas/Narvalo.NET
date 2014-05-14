// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using Narvalo.Mvp.Web.Internal;

    public abstract class MvpHttpHandler : IHttpHandler, IView
    {
        readonly bool _throwIfNoPresenterBound;

        protected MvpHttpHandler() : this(true) { }

        protected MvpHttpHandler(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public event EventHandler Load;

        public virtual bool IsReusable
        {
            get { return false; }
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var presenterBinder = HttpPresenterBinderFactory.Create(this, context);
            presenterBinder.PerformBinding();

            OnLoad();

            presenterBinder.Release();
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
