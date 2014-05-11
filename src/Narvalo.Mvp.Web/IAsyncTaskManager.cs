// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    public interface IAsyncTaskManager
    {
        void ExecuteRegisteredAsyncTasks();

        void RegisterAsyncTask(Func<CancellationToken, Task> handler);

        void RegisterAsyncTask(Func<Task> handler);

         void RegisterAsyncTask(
            BeginEventHandler beginHandler,
            EndEventHandler endHandler,
            EndEventHandler timeoutHandler,
            object state);

        void RegisterAsyncTask(
            BeginEventHandler beginHandler,
            EndEventHandler endHandler,
            EndEventHandler timeoutHandler,
            object state,
            bool executeInParallel);
    }
}
