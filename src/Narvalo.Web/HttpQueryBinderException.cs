namespace Narvalo.Web
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class HttpQueryBinderException : Exception
    {
        string _memberName = String.Empty;

        public HttpQueryBinderException() : base() { }

        public HttpQueryBinderException(string message) : base(message) { }

        public HttpQueryBinderException(string message, Exception innerException)
            : base(message, innerException) { }

        protected HttpQueryBinderException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public string MemberName { get { return _memberName; } set { _memberName = value; } }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("MemberName", _memberName);
        }
    }
}
