// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public class AggregateSentinelException : SentinelException
    {
        public AggregateSentinelException() : base() { }

        public AggregateSentinelException(string message) : base(message) { }

        public AggregateSentinelException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
