namespace Narvalo.Fx
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class OutcomeException : Exception
    {
        public OutcomeException() : base() { }

        public OutcomeException(string message) : base(message) { }

        public OutcomeException(string message, Exception innerException)
            : base(message, innerException) { }

        protected OutcomeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
