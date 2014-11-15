namespace Narvalo.Reliability
{
    public interface IMonitorableGuard : IGuard
    {
        GuardStatus Status { get; }
    }
}
