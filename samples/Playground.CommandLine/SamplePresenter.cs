// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Narvalo.Mvp;

    public sealed class SamplePresenter : Presenter<ISampleView>
    {
        public SamplePresenter(ISampleView view)
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
