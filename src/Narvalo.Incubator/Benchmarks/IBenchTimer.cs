namespace Narvalo.Benchmarks
{
    using NodaTime;

    public interface IBenchTimer
    {
        void Reset();

        Duration ElapsedTime { get; }
    }
}