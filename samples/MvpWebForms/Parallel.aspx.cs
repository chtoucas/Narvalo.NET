// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Web.UI;

    public partial class ParallelPage : Page
    {
        Stopwatch _stopWatch = new Stopwatch();

        public ParallelPage()
            : base()
        {
            Load += Page_Load;
            PreRenderComplete += Page_PreRenderComplete;
        }

        void Page_Load(object sender, EventArgs e)
        {
            _stopWatch.Start();
            ParallelControl.Model.Append("Page Load");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            _stopWatch.Stop();
            ParallelControl.Model.Append(String.Format(
                CultureInfo.InvariantCulture,
                "Page PreRenderComplete. Elapsed time: {0}s",
                _stopWatch.Elapsed.Milliseconds / 1000.0));
        }
    }
}
