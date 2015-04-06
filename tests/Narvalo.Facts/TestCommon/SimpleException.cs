// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.TestCommon
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SimpleException : Exception
    {
        public SimpleException() : base() { }

        public SimpleException(string message) : base(message) { }

        public SimpleException(string message, Exception innerException)
            : base(message, innerException) { }

        protected SimpleException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
