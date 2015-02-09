// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpCommandLine
{
    using System;
    using Narvalo.Mvp;

    public sealed class SamplePresenter : Presenter<ISampleView>
    {
        public SamplePresenter(ISampleView view)
            : base(view)
        {
            View.Load += View_Load_;
            View.Completed += View_Completed;
        }

        void View_Load_(object sender, EventArgs e)
        {
            View.ShowLoad();
        }

        void View_Completed(object sender, EventArgs e)
        {
            View.ShowCompleted();
        }
    }
}
