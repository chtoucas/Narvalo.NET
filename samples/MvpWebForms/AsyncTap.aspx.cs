// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms
{
    using System;
    using System.Web.UI;

    public partial class AsyncTapPage : Page
    {
        public AsyncTapPage()
            : base()
        {
            PreRenderComplete += Page_PreRenderComplete;
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            AsyncTapControl.Model.RecordPagePreRenderComplete();
        }
    }
}
