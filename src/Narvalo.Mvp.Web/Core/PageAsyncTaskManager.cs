// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.UI;

    using Narvalo;

    public partial class PageAsyncTaskManager : IAsyncTaskManager
    {
        private readonly Page _page;

        public PageAsyncTaskManager(Page page)
        {
            Require.NotNull(page, nameof(page));

            _page = page;
        }

        public void ExecuteRegisteredAsyncTasks()
           => _page.ExecuteRegisteredAsyncTasks();

        public void RegisterAsyncTask(Func<CancellationToken, Task> handler)
            => _page.RegisterAsyncTask(new PageAsyncTask(handler));

        public void RegisterAsyncTask(Func<Task> handler)
            => _page.RegisterAsyncTask(new PageAsyncTask(handler));

        public void RegisterAsyncTask(
            BeginEventHandler beginHandler,
            EndEventHandler endHandler,
            object state)
            => _page.RegisterAsyncTask(
                new PageAsyncTask(beginHandler, endHandler, null, state, executeInParallel: false));

        public void RegisterAsyncTask(
            BeginEventHandler beginHandler,
            EndEventHandler endHandler,
            EndEventHandler timeoutHandler,
            object state)
            => _page.RegisterAsyncTask(
                new PageAsyncTask(beginHandler, endHandler, timeoutHandler, state));

        public void RegisterAsyncTask(
            BeginEventHandler beginHandler,
            EndEventHandler endHandler,
            EndEventHandler timeoutHandler,
            object state,
            bool executeInParallel)
            => _page.RegisterAsyncTask(
                new PageAsyncTask(beginHandler, endHandler, timeoutHandler, state, executeInParallel));
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp.Web.Core
{
    using System.Diagnostics.Contracts;

    public partial class PageAsyncTaskManager
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_page != null);
        }
    }
}

#endif
