// Emprunté à Rx.NET.

namespace Narvalo.Internal
{
    using System;
    using System.Runtime.ExceptionServices;

    class DefaultExceptionServices/*Impl*/ : IExceptionServices
    {
        public void Rethrow(Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
