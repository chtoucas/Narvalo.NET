namespace Playground.Presenters
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.Views;

    public sealed class AsyncTapPresenter : HttpPresenterOf<AsyncModel>
    {
        static readonly Action Thunk_ = () => Thread.Sleep(100);

        public AsyncTapPresenter(IView<AsyncModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.RecordViewLoad();

            AsyncManager.RegisterAsyncTask(InvokeAsync);
        }

        async Task InvokeAsync()
        {
            View.Model.RecordAsyncStarted();

            await Task.Factory.FromAsync(Thunk_.BeginInvoke, Thunk_.EndInvoke, null);

            View.Model.RecordAsyncEnded();
        }
    }
}