namespace Narvalo.Reliability
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BarrierException : GuardException
    {
        public BarrierException() : base() { ; }

        public BarrierException(string message) : base(message) { ; }

        public BarrierException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected BarrierException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
