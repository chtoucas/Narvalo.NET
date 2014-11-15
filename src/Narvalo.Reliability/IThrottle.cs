namespace Narvalo.Reliability
{
    public interface IThrottle : IBarrier
    {
        bool IsConstricted { get; }

        bool IsObstructed { get; }
    }
}
