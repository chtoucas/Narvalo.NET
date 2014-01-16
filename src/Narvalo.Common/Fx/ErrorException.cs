namespace Narvalo.Fx
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ErrorException : Exception
    {
        public ErrorException() : base() { }

        public ErrorException(string message) : base(message) { ; }

        public ErrorException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected ErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
