// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Web.UI;
    using Narvalo.Mvp.Web.Internal;

    public abstract class MvpUserControl : UserControl, IView
    {
        readonly bool _throwIfNoPresenterBound;

        protected MvpUserControl() : this(true) { }

        protected MvpUserControl(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        protected override void OnInit(EventArgs e)
        {
            // REVIEW: Remove the following check.
            if (Page == null) {
                throw new InvalidOperationException(
                    "Controls can only be registered once they have been added to the live control tree.");
            }

            PageHost.Register(Page, Context).RegisterView(this);

            base.OnInit(e);
        }
    }
}
