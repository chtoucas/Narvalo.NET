namespace Playground.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.Views;

    public sealed class TestPresenter : Presenter<ITestView>, IDisposable
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

        public void Dispose()
        {
            View.Completed -= Completed;
            View.Load -= Load;
        }
    }
}
