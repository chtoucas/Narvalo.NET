namespace Narvalo
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NilException : Exception
    {
        public NilException() : base() { }

        public NilException(string message) : base(message) { }

        public NilException(string message, Exception innerException)
            : base(message, innerException) { }

        protected NilException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
