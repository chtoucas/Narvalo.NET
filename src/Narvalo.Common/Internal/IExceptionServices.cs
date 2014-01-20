// Emprunté à Rx.NET.

namespace Narvalo.Internal
{
    using System;
    using System.ComponentModel;

    [EditorBrowsable(EditorBrowsableState.Never)]
    interface IExceptionServices
    {
        void Rethrow(Exception exception);
    }
}
