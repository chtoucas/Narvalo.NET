namespace Narvalo.Benchmark
{
    using NodaTime;

    public interface IBenchTimer
    {
        void Reset();

        Duration ElapsedTime { get; }
    }
}