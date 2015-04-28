// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public class RequestCounter : ISentinel, IReliabilityMonitor
    {
        private long _failureCount = 0;
        private long _requestCount = 0;
        private long _successCount = 0;

        public long FailureCount
        {
            get { return _failureCount; }
        }

        public long RequestCount
        {
            get { return _requestCount; }
        }

        public long SuccessCount
        {
            get { return _successCount; }
        }

        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            RecordRequest();

            try
            {
                action.Invoke();
            }
            catch (GuardException)
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

        public void RecordFailure()
        {
            _failureCount++;
        }

        public void RecordRequest()
        {
            _requestCount++;
        }

        public void RecordSuccess()
        {
            _successCount++;
        }
    }
}
