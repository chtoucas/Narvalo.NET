namespace Playground.Presenters
{
    using System;
    using Narvalo;
    using Narvalo.Mvp;
    using Playground.Views;

    public sealed class TestPresenter : IPresenter<ITestView>, IDisposable
    {
        readonly ITestView _view;

        public TestPresenter(ITestView view)
        {
            Require.NotNull(view, "view");

            _view = view;

            View.Completed += Completed;
            View.Load += Load;
        }

        public ITestView View { get { return _view; } }

        public IMessageBus Messages { get; set; }

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
