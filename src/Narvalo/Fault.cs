namespace Narvalo
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class Fault
    {
        readonly Exception _exception;

        public Fault(Exception exception)
        {
            Requires.NotNull(exception, "exception");

            _exception = exception;
        }

        public Fault(string errorMessage)
        {
            _exception = new FaultException(errorMessage);
        }

        public Exception Exception { get { return _exception; } }

        [Serializable]
        class FaultException : Exception
        {
            public FaultException() : base() { }

            public FaultException(string message) : base(message) { ; }

            public FaultException(string message, Exception innerException)
                : base(message, innerException) { ; }

            protected FaultException(SerializationInfo info, StreamingContext context)
                : base(info, context) { ; }
        }
    }
}
