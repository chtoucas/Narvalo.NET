namespace Narvalo.Internal
{
    using System;
    using System.ComponentModel;

    [Alien(AlienSource.Library, 
        GenuineName = "System.Reactive.IExceptionServices",
        Link = "https://github.com/Reactive-Extensions/Rx.NET")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    interface IExceptionServices
    {
        void Rethrow(Exception exception);
    }
}
