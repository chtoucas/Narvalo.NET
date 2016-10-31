// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class HttpQueryBinderException : Exception
    {
        public HttpQueryBinderException() { }

        public HttpQueryBinderException(string message) : base(message) { }

        public HttpQueryBinderException(string message, Exception innerException)
            : base(message, innerException) { }

        protected HttpQueryBinderException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public string MemberName { get; set; } = String.Empty;

#if SECURITY_ANNOTATIONS
        [System.Security.SecurityCritical]
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("MemberName", MemberName);
        }
    }
}
