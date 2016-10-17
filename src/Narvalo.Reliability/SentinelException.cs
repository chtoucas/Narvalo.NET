// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public class SentinelException : Exception
    {
        public SentinelException() : base() { }

        public SentinelException(string message) : base(message) { }

        public SentinelException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
