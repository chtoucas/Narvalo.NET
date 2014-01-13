namespace Narvalo.Mail {
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class Pop3ClientException : Exception {
        public Pop3ClientException() : base() { ; }

        public Pop3ClientException(string message) : base(message) { ; }

        public Pop3ClientException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected Pop3ClientException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
