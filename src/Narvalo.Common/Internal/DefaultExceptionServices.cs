namespace Narvalo.Internal
{
    using System;
    using System.Runtime.ExceptionServices;

    [Alien(AlienSource.Library,
        GenuineName = "System.Reactive.PlatformServices.DefaultExceptionServices",
        Link = "https://github.com/Reactive-Extensions/Rx.NET")]
    sealed class DefaultExceptionServices/*Impl*/ : IExceptionServices
    {
        public void Rethrow(Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
