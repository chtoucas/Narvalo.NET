namespace Narvalo.Fx
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FailureException : Exception
    {
        public FailureException() : base() { }

        public FailureException(string message) : base(message) { }

        public FailureException(string message, Exception innerException)
            : base(message, innerException) { }

        protected FailureException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
