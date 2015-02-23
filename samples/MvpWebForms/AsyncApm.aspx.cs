// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.UI;

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apm")]
    public partial class AsyncApmPage : Page
    {
        public AsyncApmPage()
            : base()
        {
            PreRenderComplete += Page_PreRenderComplete;
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            AsyncApmControl.Model.RecordPagePreRenderComplete();
        }
    }
}
