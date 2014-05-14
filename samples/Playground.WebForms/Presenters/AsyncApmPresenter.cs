namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Common;
    using Playground.WebForms.Views.Models;

    public class AsyncApmPresenter : HttpPresenterOf<AsyncMessagesModel>
    {
        public AsyncApmPresenter(IView<AsyncMessagesModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Append("View.Load");

            AsyncManager.RegisterAsyncTask(BeginAsyncA, EndAsyncA, null, null, true);
            AsyncManager.RegisterAsyncTask(BeginAsyncB, EndAsyncB, null, null, true);
        }

        IAsyncResult BeginAsyncA(object sender, EventArgs e, AsyncCallback cb, object state)
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

        IAsyncResult BeginAsyncB(object sender, EventArgs e, AsyncCallback cb, object state)
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