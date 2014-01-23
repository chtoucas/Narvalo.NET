namespace Narvalo
{
    using System;
    using Narvalo.Internal;

    [Alien(AlienSource.Library,
        GenuineName = "System.Reactive.ExceptionHelpers",
        Link = "https://github.com/Reactive-Extensions/Rx.NET")]
    public static class ExceptionExtensions
    {
        static Lazy<IExceptionServices> Services_ = new Lazy<IExceptionServices>(Initialize_);

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

        static IExceptionServices Initialize_()
        {
            return new DefaultExceptionServices();
        }
    }
}
