namespace Playground.WebForms.Presenters
{
    using System;
    using System.Threading.Tasks;
    using Narvalo.Mvp;
    using Playground.WebForms.Common;
    using Playground.WebForms.Views.Models;

    public class AsyncTplPresenter : PresenterOf<AsyncMessagesModel>
    {
        public AsyncTplPresenter(IView<AsyncMessagesModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Append("View.Load");

            TaskA();
            TaskB();
        }

        async void TaskA()
        {
            await Task.Factory.FromAsync(BeginAsyncA, EndAsyncA, null);
        }

        async void TaskB()
        {
            await Task.Factory.FromAsync(BeginAsyncB, EndAsyncB, null);
        }

        IAsyncResult BeginAsyncA(AsyncCallback cb, object state)
        {
            View.Model.Append("Async task A started");
            return AsyncThunk.A.BeginInvoke(cb, state);
        }

        void EndAsyncA(IAsyncResult ar)
        {
            var msg = AsyncThunk.A.EndInvoke(ar);
            View.Model.Append(msg);
            View.Model.Append("Async task A ended");
        }

        IAsyncResult BeginAsyncB(AsyncCallback cb, object state)
        {
            View.Model.Append("Async task B started");
            return AsyncThunk.B.BeginInvoke(cb, state);
        }

        void EndAsyncB(IAsyncResult ar)
        {
            var msg = AsyncThunk.B.EndInvoke(ar);
            View.Model.Append(msg);
            View.Model.Append("Async task B ended");
        }
    }
}