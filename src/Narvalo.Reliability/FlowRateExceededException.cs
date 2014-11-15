namespace Narvalo.Reliability
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FlowRateExceededException : BarrierException
    {
        public FlowRateExceededException() : base() { ; }

        public FlowRateExceededException(string message) : base(message) { ; }

        public FlowRateExceededException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected FlowRateExceededException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
