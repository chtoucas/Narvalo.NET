// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class FlowRateExceededException : BarrierException
    {
        public FlowRateExceededException() : base() { ; }

        public FlowRateExceededException(string message) : base(message) { ; }

        public FlowRateExceededException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected FlowRateExceededException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
