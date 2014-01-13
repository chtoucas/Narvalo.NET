namespace Narvalo.Benchmark
{
    using System.Diagnostics;
    using NodaTime;

    public class BenchTimer : IBenchTimer
    {
        readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public void Reset()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public Duration ElapsedTime
        {
            get { return Duration.FromTicks(_stopwatch.Elapsed.Ticks); }
        }
    }
}