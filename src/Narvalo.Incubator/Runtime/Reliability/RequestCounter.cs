namespace Narvalo.Runtime.Reliability
{
    using System;

    public class RequestCounter : IGuard, IReliabilityMonitor
    {
        long _failureCount = 0;
        long _requestCount = 0;
        long _successCount = 0;

        #region IGuard

        public void Execute(Action action)
        {
            Requires.NotNull(action, "action");

            RecordRequest();

            try {
                action();
            }
            catch (GuardException) {
                throw;
            }
            catch {
                RecordFailure();
                throw;
            }

            RecordSuccess();
        }

        #endregion

        #region IReliabilityMonitor

        public long FailureCount
        {
            get { return _failureCount; }
            private set { _failureCount = value; }
        }

        public long RequestCount
        {
            get { return _requestCount; }
            private set { _requestCount = value; }
        }

        public long SuccessCount
        {
            get { return _successCount; }
            private set { _successCount = value; }
        }

        public void RecordFailure()
        {
            FailureCount++;
        }

        public void RecordRequest()
        {
            RequestCount++;
        }

        public void RecordSuccess()
        {
            SuccessCount++;
        }

        #endregion
    }
}
