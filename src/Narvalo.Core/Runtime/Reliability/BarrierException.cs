// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;

    public class BarrierException : GuardException
    {
        public BarrierException() : base() { }

        public BarrierException(string message) : base(message) { }

        public BarrierException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
