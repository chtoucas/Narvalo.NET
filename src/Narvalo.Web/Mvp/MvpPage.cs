// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Mvp
{
    using System;
    using System.Web.UI;
    using Narvalo.Mvp;
    using Narvalo.Web.Mvp.Core;

    public abstract class MvpPage : Page, IView
    {
        readonly bool _throwIfNoPresenterBound;

        bool _autoDataBind = true;

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
