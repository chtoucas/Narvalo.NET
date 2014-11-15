namespace Narvalo.Reliability
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CircuitOpenException : BarrierException
    {
        public CircuitOpenException() : base() { ; }

        public CircuitOpenException(string message) : base(message) { ; }

        public CircuitOpenException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected CircuitOpenException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
