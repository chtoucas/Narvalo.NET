// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.UI;

    public class PageAsyncTaskManager : IAsyncTaskManager
    {
        private readonly Page _page;

        public PageAsyncTaskManager(Page page)
        {
            _page = page;
        }

        public void ExecuteRegisteredAsyncTasks()
        {
            _page.ExecuteRegisteredAsyncTasks();
        }

        public void RegisterAsyncTask(Func<CancellationToken, Task> handler)
        {
            _page.RegisterAsyncTask(new PageAsyncTask(handler));
        }

        public void RegisterAsyncTask(Func<Task> handler)
        {
            _page.RegisterAsyncTask(new PageAsyncTask(handler));
        }

        public void RegisterAsyncTask(
            BeginEventHandler beginHandler,
            EndEventHandler endHandler,
            object state)
        {
            _page.RegisterAsyncTask(
                new PageAsyncTask(beginHandler, endHandler, null, state, executeInParallel: false));
        }

        public void RegisterAsyncTask(
            BeginEventHandler beginHandler, 
            EndEventHandler endHandler,
            EndEventHandler timeoutHandler,
            object state)
        {
            _page.RegisterAsyncTask(
                new PageAsyncTask(beginHandler, endHandler, timeoutHandler, state));
        }

        public void RegisterAsyncTask(
            BeginEventHandler beginHandler,
            EndEventHandler endHandler, 
            EndEventHandler timeoutHandler, 
            object state,
            bool executeInParallel)
        {
            _page.RegisterAsyncTask(
                new PageAsyncTask(beginHandler, endHandler, timeoutHandler, state, executeInParallel));
        }
    }
}
