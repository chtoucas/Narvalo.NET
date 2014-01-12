namespace Narvalo.Runtime.Reliability
{
    public interface IMonitorableGuard : IGuard
    {
        GuardStatus Status { get; }
    }
}
