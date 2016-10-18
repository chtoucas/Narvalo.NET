// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public class ReliabilityException : Exception
    {
        public ReliabilityException() : base() { }

        public ReliabilityException(string message) : base(message) { }

        public ReliabilityException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
