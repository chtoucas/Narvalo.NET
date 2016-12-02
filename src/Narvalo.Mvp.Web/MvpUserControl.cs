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
        private readonly bool _throwIfNoPresenterBound;

        protected MvpUserControl() : this(true) { }

        protected MvpUserControl(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

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
            Demand.NotNull(Page);
            Ensures(Result<T>() != null);

            var t = Page.GetDataItem() as T;
            return t ?? new T();
        }

        protected T DataValue<T>() where T : class
        {
            Demand.NotNull(Page);

            // NB: Originally WebFormsMvp does not add a type constraint and use a direct cast:
            // (T)Page.GetDataItem() but it seems a bit dangerous to me (InvalidCastException).
            return Page.GetDataItem() as T;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "XXX")]
        protected string DataValue<T>(string format) where T : class
        {
            Demand.NotNull(Page);
            Ensures(Result<string>() != null);

            // NB: Originally WebFormsMvp does not add a type constraint and use a direct cast:
            // (T)Page.GetDataItem() but it seems a bit dangerous to me (InvalidCastException).
            return Format.Current(format, Page.GetDataItem() as T);
        }
    }
}
