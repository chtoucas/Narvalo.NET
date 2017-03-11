// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.UI;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Core;

    using static System.Diagnostics.Contracts.Contract;

    public abstract class MvpUserControl : UserControl, IView
    {
        protected MvpUserControl() : this(true) { }

        protected MvpUserControl(bool throwIfNoPresenterBound)
        {
            ThrowIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public bool ThrowIfNoPresenterBound { get; }

        protected bool AutoDataBind { get; set; } = true;

        protected override void OnInit(EventArgs e)
        {
            Assume(Page != null, "Extern: ASP.NET.");
            PageHost.Register(Page, Context).RegisterView(this);

            if (AutoDataBind)
            {
                Page.PreRenderComplete += (sender, args) => DataBind();
            }

            base.OnInit(e);
        }

        protected T DataItem<T>() where T : class, new()
        {
            var t = Page.GetDataItem() as T;
            return t ?? new T();
        }

        protected T DataValue<T>() where T : class
        {
            // NB: WebFormsMvp does not add a type constraint and use a direct cast
            // (T)Page.GetDataItem() but I prefer to avoid boxing/unboxing and the risk
            // of an InvalidCastException.
            return Page.GetDataItem() as T;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "XXX")]
        protected string DataValue<T>(string format) where T : class
        {
            // NB: WebFormsMvp does not add a type constraint and use a direct cast
            // (T)Page.GetDataItem() but I prefer to avoid boxing/unboxing and the risk
            // of an InvalidCastException.
            return Format.Current(format, Page.GetDataItem() as T);
        }
    }
}
