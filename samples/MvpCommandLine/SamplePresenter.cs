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
            View.Load += View_Load;
            View.Completed += View_Completed;
        }

        private void View_Load(object sender, EventArgs e) => View.ShowLoad();

        private void View_Completed(object sender, EventArgs e) => View.ShowCompleted();
    }
}
