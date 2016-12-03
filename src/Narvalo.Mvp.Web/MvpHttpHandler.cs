// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using Narvalo.Mvp.Web.Internal;

    using static System.Diagnostics.Contracts.Contract;

    [Obsolete("Using MVP for an HTTP Handler is supported but discouraged.")]
    public abstract class MvpHttpHandler : IHttpHandler, IView
    {
        protected MvpHttpHandler() : this(true) { }

        protected MvpHttpHandler(bool throwIfNoPresenterBound)
        {
            ThrowIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public event EventHandler Load;

        public virtual bool IsReusable
        {
            get
            {
                Ensures(Result<bool>() == false);

                return false;
            }
        }

        public bool ThrowIfNoPresenterBound { get; }

        public void ProcessRequest(HttpContext context)
        {
            var presenterBinder = HttpPresenterBinderFactory.Create(this, context);
            presenterBinder.PerformBinding();

            OnLoad();

            presenterBinder.Release();
        }

        protected virtual void OnLoad() => Load?.Invoke(this, EventArgs.Empty);
    }
}