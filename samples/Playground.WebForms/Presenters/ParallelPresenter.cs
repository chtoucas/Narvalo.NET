namespace Playground.Presenters
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.Views;

    public sealed class ParallelPresenter : HttpPresenterOf<ConcurrentModel>
    {
        static readonly Func<string, string> Thunk_ = (string name) =>
        {
            Thread.Sleep(100);

            return String.Format("Task {0} ended", name);
        };

        public ParallelPresenter(IView<ConcurrentModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Append("View Load");

            AsyncManager.RegisterAsyncTask(RunTasks);
        }

        async Task RunTasks()
        {
            await Task.WhenAll(CreateTask("A"), CreateTask("B"), CreateTask("C"));
        }

        Task CreateTask(string name)
        {
            return Task.Factory.FromAsync(BeginInvoke, EndInvoke, name);
        }

        IAsyncResult BeginInvoke(AsyncCallback cb, object state)
        {
            View.Model.Append(String.Format("Task {0} started", state));

            return Thunk_.BeginInvoke((string)state, cb, state);
        }

        void EndInvoke(IAsyncResult ar)
        {
            var result = Thunk_.EndInvoke(ar);

            View.Model.Append(result);
        }
    }
}