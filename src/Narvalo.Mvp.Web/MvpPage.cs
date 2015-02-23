// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web.UI;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Core;

    public abstract class MvpPage : Page, IView
    {
        private readonly bool _throwIfNoPresenterBound;

        private bool _autoDataBind = true;

        protected MvpPage() : this(true) { }

        protected MvpPage(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        protected bool AutoDataBind
        {
            get { return _autoDataBind; }
            set { _autoDataBind = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            PageHost.Register(this, Context);

            base.OnInit(e);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (AutoDataBind) {
                DataBind();
            }

            base.OnPreRenderComplete(e);
        }
    }
}
