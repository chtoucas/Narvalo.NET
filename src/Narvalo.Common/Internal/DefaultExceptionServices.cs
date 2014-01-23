namespace Narvalo.Internal
{
    using System;
    using System.Runtime.ExceptionServices;

    [Alien("System.Reactive.PlatformServices.DefaultExceptionServices")]
    class DefaultExceptionServices/*Impl*/ : IExceptionServices
    {
        public void Rethrow(Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
