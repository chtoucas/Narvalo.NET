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
            Console.WriteLine("Completed.");
        }

        public void Load(object sender, EventArgs e)
        {
            Console.WriteLine("Load.");
        }
    }
}
