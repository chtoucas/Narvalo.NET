// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    public interface IReliabilityMonitor
    {
        long FailureCount { get; }
        long SuccessCount { get; }
        long RequestCount { get; }

        void RecordFailure();
        void RecordSuccess();
        void RecordRequest();
    }
}
