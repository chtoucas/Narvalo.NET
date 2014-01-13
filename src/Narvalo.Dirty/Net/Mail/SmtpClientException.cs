namespace Narvalo.Mail
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SmtpClientException : Exception
    {
        public SmtpClientException() : base() { ; }

        public SmtpClientException(string message) : base(message) { ; }

        public SmtpClientException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected SmtpClientException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
