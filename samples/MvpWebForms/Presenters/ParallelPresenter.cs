// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Presenters
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using MvpWebForms.Views;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;

    public sealed class ParallelPresenter : HttpPresenterOf<ConcurrentModel>
    {
        static readonly Func<string, string> Thunk_ = (string name) =>
        {
            Thread.Sleep(100);

            return String.Format(CultureInfo.InvariantCulture, "Task {0} ended. ", name);
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
            // NB: This is just to demonstrate parallel execution of tasks.
            // In real world, you are better off using Parallel.For:
            ////Parallel.For(0, 3, i =>
            ////{
            ////    View.Model.Append(String.Format("Task {0} started", i));
            ////    Thread.Sleep(100);
            ////    View.Model.Append(String.Format("Task {0} ended. ", i));
            ////});

            await Task.WhenAll(CreateTask("A"), CreateTask("B"), CreateTask("C"));
        }

        Task CreateTask(string name)
        {
            return Task.Factory.FromAsync(BeginInvoke, EndInvoke, name);
        }

        IAsyncResult BeginInvoke(AsyncCallback cb, object state)
        {
            View.Model.Append(String.Format(CultureInfo.InvariantCulture, "Task {0} started", state));

            return Thunk_.BeginInvoke((string)state, cb, state);
        }

        void EndInvoke(IAsyncResult ar)
        {
            var result = Thunk_.EndInvoke(ar);

            View.Model.Append(result);
        }
    }
}
