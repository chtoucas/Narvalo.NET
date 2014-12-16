namespace Narvalo.Reliability
{
    using System;

    [Serializable]
    public class AggregateGuardException : GuardException
    {
        public AggregateGuardException(string message) : base(message) { }

        public AggregateGuardException(string message, AggregateException innerException)
            : base(message, innerException) { }
    }
}
