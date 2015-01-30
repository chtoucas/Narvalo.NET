// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpCommandLine
{
    using System;
    using Narvalo.Mvp;

    public sealed class SamplePresenter : Presenter<ISampleView>, IDisposable
    {
        public SamplePresenter(ISampleView view)
            : base(view)
        {
            View.Load += Load_;
            View.Completed += Completed_;
        }

        public void Dispose()
        {
            View.Load -= Load_;
            View.Completed -= Completed_;
        }

        void Load_(object sender, EventArgs e)
        {
            View.ShowLoad();
        }

        void Completed_(object sender, EventArgs e)
        {
            View.ShowCompleted();
        }
    }
}
