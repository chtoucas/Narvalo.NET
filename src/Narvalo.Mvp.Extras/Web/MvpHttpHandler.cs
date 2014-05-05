// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;

    public abstract class MvpHttpHandler : IHttpHandler, IView
    {
        protected MvpHttpHandler() { }

        public void ProcessRequest(HttpContext context)
        {
            var presenterBinder = new AspNetPresenterBinder(this, context);
            presenterBinder.PerformBinding();

            OnLoad();

            presenterBinder.Release();
        }

        public event EventHandler Load;

        public virtual bool IsReusable
        {
            get { return false; }
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
