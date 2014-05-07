// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web.UI;
    using Narvalo.Mvp.Web.Internal;

    public abstract class MvpPage : Page, IView
    {
        readonly bool _throwIfNoPresenterBound;

        protected MvpPage() : this(true) { }

        protected MvpPage(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        protected override void OnInit(EventArgs e)
        {
            PageHost.RegisterPage(this, Context);

            base.OnInit(e);
        }
    }
}
