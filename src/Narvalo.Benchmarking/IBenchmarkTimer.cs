namespace Narvalo.Benchmarking
{
    using NodaTime;

    public interface IBenchmarkTimer
    {
        Duration ElapsedTime { get; }

        void Reset();
    }
}