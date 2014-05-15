namespace Playground.WebForms.Presenters
{
    using System;
    using System.Threading;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views;

    public class AsyncApmPresenter : HttpPresenterOf<AsyncMessagesModel>
    {
        static readonly Func<string, string> Thunk_ = (string name) =>
        {
            Thread.Sleep(100);

            return String.Format("Task {0} processed", name);
        };

        public AsyncApmPresenter(IView<AsyncMessagesModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Append("View.Load");

            AsyncManager.RegisterAsyncTask(BeginAsync, EndAsync, TimeoutAsync, "A", true);
            AsyncManager.RegisterAsyncTask(BeginAsync, EndAsync, TimeoutAsync, "B", true);
        }

        IAsyncResult BeginAsync(object sender, EventArgs e, AsyncCallback cb, object state)
        {
            View.Model.Append(String.Format("Async task {0} started", state));

            return Thunk_.BeginInvoke((string)state, cb, state);
        }

        void EndAsync(IAsyncResult ar)
        {
            var result = Thunk_.EndInvoke(ar);

            View.Model.Append(result);
            View.Model.Append(String.Format("Task {0} ended", ar.AsyncState));
        }

        void TimeoutAsync(IAsyncResult ar)
        {
            View.Model.Append(String.Format("Task {0} timed out", ar.AsyncState));
        }
    }
}