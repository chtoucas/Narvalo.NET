namespace Narvalo.Runtime.Reliability
{
    public interface IThrottle : IBarrier
    {
        bool IsConstricted { get; }

        bool IsObstructed { get; }
    }
}
