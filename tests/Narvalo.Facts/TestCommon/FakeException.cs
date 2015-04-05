// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.TestCommon
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FakeException : Exception
    {
        public FakeException() : base() { }

        public FakeException(string message) : base(message) { }

        public FakeException(string message, Exception innerException)
            : base(message, innerException) { }

        protected FakeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
