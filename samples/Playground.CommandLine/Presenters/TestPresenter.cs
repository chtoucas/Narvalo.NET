// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.Views;

    public sealed class TestPresenter : Presenter<ITestView>
    {
        public TestPresenter(ITestView view)
            : base(view)
        {
            View.Completed += Completed;
            View.Load += Load;
        }

        public void Completed(object sender, EventArgs e)
        {
            Console.WriteLine("Presenter say Completed.");
        }

        public void Load(object sender, EventArgs e)
        {
            Console.WriteLine("Presenter say Load.");
        }
    }
}
