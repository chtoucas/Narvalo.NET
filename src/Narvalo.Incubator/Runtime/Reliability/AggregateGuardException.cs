namespace Narvalo.Runtime.Reliability
{
    using System;

    public class AggregateGuardException : GuardException
    {
        public AggregateGuardException(string message) : base(message) { }

        public AggregateGuardException(string message, AggregateException innerException)
            : base(message, innerException) { ; }
    }
}
