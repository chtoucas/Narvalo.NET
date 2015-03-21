// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;

    public class FlowRateExceededException : BarrierException
    {
        public FlowRateExceededException() : base() { }

        public FlowRateExceededException(string message) : base(message) { }

        public FlowRateExceededException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
