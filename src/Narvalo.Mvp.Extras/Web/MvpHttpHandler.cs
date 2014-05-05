// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using Narvalo.Mvp.PresenterBinding;

    public abstract class MvpHttpHandler : IHttpHandler, IView
    {
        protected MvpHttpHandler() { }

        public void ProcessRequest(HttpContext context)
        {
            var presenterBinder = new PresenterBinder(this);
            presenterBinder.PresenterCreated += (sender, e) =>
            {
                var presenter = e.Presenter as IHttpPresenter;
                if (presenter != null) {
                    presenter.HttpContext = new HttpContextWrapper(context);
                }
            };

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
