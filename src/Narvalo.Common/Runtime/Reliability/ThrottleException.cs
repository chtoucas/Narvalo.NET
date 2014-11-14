namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ThrottleException : GuardException
    {
        public ThrottleException() : base() { ; }

        public ThrottleException(string message) : base(message) { ; }

        public ThrottleException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected ThrottleException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
