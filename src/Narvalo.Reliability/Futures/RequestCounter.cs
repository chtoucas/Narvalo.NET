// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public sealed class RequestCounter : IReliabilitySentinel, IReliabilityMonitor
    {
        public long FailureCount { get; private set; } = 0;

        public long RequestCount { get; private set; } = 0;

        public long SuccessCount { get; private set; } = 0;

        public void Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));

            RecordRequest();

            try
            {
                action.Invoke();
            }
            catch (SentinelException)
            {
                throw;
            }
            catch
            {
                RecordFailure();
                throw;
            }

            RecordSuccess();
        }

        public void RecordFailure() => FailureCount++;

        public void RecordRequest() => RequestCount++;

        public void RecordSuccess() => SuccessCount++;
    }
}
