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
            await Task.Factory.FromAsync(BeginInvoke, EndInvoke, null);
        }

        IAsyncResult BeginInvoke(AsyncCallback cb, object state)
        {
            View.Model.RecordAsyncStarted();

            return Thunk_.BeginInvoke(cb, state);
        }

        void EndInvoke(IAsyncResult ar)
        {
            Thunk_.EndInvoke(ar);

            View.Model.RecordAsyncEnded();
        }
    }
}