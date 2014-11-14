namespace Narvalo.Runtime.Reliability
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
