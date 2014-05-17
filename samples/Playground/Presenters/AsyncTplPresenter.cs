namespace Playground.Presenters
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Narvalo.Mvp;
    using Playground.Views;

    // WARNING: DO NOT use async on void event handlers like Load().
    // Don't forget to mark the page with Async="true".
    // In the Web.config, (this is the default)
    // <appSettings>
    //   <add key="aspnet:UseTaskFriendlySynchronizationContext" value="false" />
    // </appSettings>
    // References:
    // - http://www.hanselman.com/blog/TheMagicOfUsingAsynchronousMethodsInASPNET45PlusAnImportantGotcha.aspx
    // - http://msdn.microsoft.com/en-us/library/hh975440.aspx for UseTaskFriendlySynchronizationContext
    // NB: Using the TPL allows to reuse the presenter in a non web context (we do not inherit from HttpPresenter).
    public sealed class AsyncTplPresenter : PresenterOf<AsyncMessagesModel>
    {
        static readonly Func<string, string> Thunk_ = (string name) =>
        {
            Thread.Sleep(100);

            return String.Format("Task {0} processed", name);
        };

        public AsyncTplPresenter(IView<AsyncMessagesModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Append("View.Load");

            RunTask("A");
            RunTask("B");

            // NB: To use the AsyncManager.RegisterAsyncTask we must enable 
            // UseTaskFriendlySynchronizationContext but this conflicts with 
            // the APM style of doing async stuff.
        }

        async void RunTask(string state)
        {
            await Task.Factory.FromAsync(BeginAsync, EndAsync, state);
        }

        IAsyncResult BeginAsync(AsyncCallback cb, object state)
        {
            View.Model.Append(String.Format("Task {0} started", state));

            return Thunk_.BeginInvoke((string)state, cb, state);
        }

        void EndAsync(IAsyncResult ar)
        {
            var result = Thunk_.EndInvoke(ar);

            View.Model.Append(result);
            View.Model.Append(String.Format("Task {0} ended", ar.AsyncState));
        }
    }
}