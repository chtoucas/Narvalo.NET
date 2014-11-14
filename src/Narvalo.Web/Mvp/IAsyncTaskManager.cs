// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Mvp
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

        void RegisterAsyncTask(BeginEventHandler beginHandler, EndEventHandler endHandler, object state);

        [Obsolete("As of .NET 4.5, you should not provide a timeoutHandler. Instead you should use one of the Task-based overloads.")]
        void RegisterAsyncTask(
           BeginEventHandler beginHandler,
           EndEventHandler endHandler,
           EndEventHandler timeoutHandler,
           object state);

        [Obsolete("As of .NET 4.5, you should not provide a timeoutHandler or set executeInParallel to true. Instead you should use one of the Task-based overloads.")]
        void RegisterAsyncTask(
            BeginEventHandler beginHandler,
            EndEventHandler endHandler,
            EndEventHandler timeoutHandler,
            object state,
            bool executeInParallel);
    }
}
