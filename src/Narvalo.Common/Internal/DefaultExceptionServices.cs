namespace Narvalo.Internal
{
    using System;
    using System.Runtime.ExceptionServices;

    [TypeBorrowedFrom("System.Reactive.PlatformServices.DefaultExceptionServices")]
    sealed class DefaultExceptionServices/*Impl*/ : IExceptionServices
    {
        public void Rethrow(Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
