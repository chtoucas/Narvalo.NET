// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web;
    using Narvalo.Mvp.Web.Internal;

    //public abstract class MvpHttpHandler<TViewModel> : MvpHttpHandler, IView<TViewModel>
    //{
    //    TViewModel _model;

    //    protected MvpHttpHandler() : base(true) { }

    //    protected MvpHttpHandler(bool throwIfNoPresenterBound) : base(throwIfNoPresenterBound) { }

    //    public TViewModel Model
    //    {
    //        get
    //        {
    //            if (_model == null) {
    //                throw new InvalidOperationException(
    //                    "The Model property is currently null, however it should have been automatically initialized by the presenter. This most likely indicates that no presenter was bound to the control. Check your presenter bindings.");
    //            }

    //            return _model;
    //        }
    //        set { _model = value; }
    //    }
    //}

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
