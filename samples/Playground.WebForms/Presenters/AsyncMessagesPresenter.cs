namespace Playground.WebForms.Presenters
{
    using System;
    using System.Threading;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views.Models;

    public class AsyncMessagesPresenter : HttpPresenterOf<AsyncMessagesModel>
    {
        static readonly Func<string> _doStuff1 = () =>
        {
            Thread.Sleep(3000);
            return ThreadMessage("Async task doStuff1 processed");
        };

        static readonly Func<string> _doStuff2 = () =>
        {
            Thread.Sleep(1500);
            return ThreadMessage("Async task doStuff2 processed");
        };

        public AsyncMessagesPresenter(IView<AsyncMessagesModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Messages.Add(ThreadMessage("View.Load event handled"));

            AsyncManager.RegisterAsyncTask(
                (asyncSender, ea, callback, state) => // Begin
                {
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff1 begin handler"));
                    return _doStuff1.BeginInvoke(callback, state);
                },
                result => // End
                {
                    var msg = _doStuff1.EndInvoke(result);
                    View.Model.Messages.Add(msg);
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff1 end handler"));
                },
                result => // Timeout
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff1 timeout handler")),
                null,
                true
            );

            AsyncManager.RegisterAsyncTask(
                (asyncSender, ea, callback, state) => // Begin
                {
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff2 begin handler"));
                    return _doStuff2.BeginInvoke(callback, state);
                },
                result => // End
                {
                    var msg = _doStuff2.EndInvoke(result);
                    View.Model.Messages.Add(msg);
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff2 end handler"));
                },
                result => // Timeout
                    View.Model.Messages.Add(ThreadMessage("Async task doStuff2 timeout handler")),
                null,
                true
            );
        }

        static string ThreadMessage(string prefix)
        {
            return String.Format(
                "{0} on thread {1} at {2}", prefix,
                Thread.CurrentThread.ManagedThreadId,
                DateTime.Now.ToString("HH:mm:ss.ss"));
        }
    }
}