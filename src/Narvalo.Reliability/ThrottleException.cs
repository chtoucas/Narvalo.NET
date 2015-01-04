// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ThrottleException : GuardException
    {
        public ThrottleException() : base() { ; }

        public ThrottleException(string message) : base(message) { ; }

        public ThrottleException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected ThrottleException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
