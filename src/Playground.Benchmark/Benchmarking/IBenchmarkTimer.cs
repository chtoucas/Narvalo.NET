namespace Playground.Benchmark.Benchmarking
{
    using NodaTime;

    public interface IBenchmarkTimer
    {
        void Reset();

        Duration ElapsedTime { get; }
    }
}