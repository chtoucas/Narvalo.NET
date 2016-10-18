// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public class AggregateReliabilityException : ReliabilityException
    {
        public AggregateReliabilityException() : base() { }

        public AggregateReliabilityException(string message) : base(message) { }

        public AggregateReliabilityException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
