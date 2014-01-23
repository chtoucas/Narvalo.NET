namespace Narvalo.Internal
{
    using System;
    using System.ComponentModel;

    [Alien("System.Reactive.IExceptionServices")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    interface IExceptionServices
    {
        void Rethrow(Exception exception);
    }
}
