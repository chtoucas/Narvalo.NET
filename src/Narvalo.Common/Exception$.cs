// Emprunté à Rx.NET.

namespace Narvalo
{
    using System;
    using Narvalo.Internal;

    public static class ExceptionExtensions
    {
        static Lazy<IExceptionServices> Services_ = new Lazy<IExceptionServices>(Initialize);

        public static void Throw(this Exception exception)
        {
            Services_.Value.Rethrow(exception);
        }

        public static void ThrowIfNotNull(this Exception exception)
        {
            if (exception != null) {
                Services_.Value.Rethrow(exception);
            }
        }

        static IExceptionServices Initialize()
        {
            return new DefaultExceptionServices();
        }
    }
}
