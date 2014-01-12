namespace Narvalo.Benchmarks.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo;
    using NodaTime;

    internal class Benchmarker
    {
        readonly IBenchTimer _timer;

        public Benchmarker(IBenchTimer timer)
        {
            Requires.NotNull(timer, "timer");

            _timer = timer;
        }

        public Duration Time(Benchmark benchmark)
        {
            Requires.NotNull(benchmark, "benchmark");

            return Time(benchmark.Action, benchmark.Iterations);
        }

        public Duration Time(Action action, int iterations)
        {
            Requires.NotNull(action, "action");

            Cleanup();

            // Echauffement (à améliorer).
            action();

            _timer.Reset();
            for (int i = 0; i < iterations; i++) {
                action();
            }
            return _timer.ElapsedTime;
        }

        [SuppressMessage("Microsoft.Reliability",
            "CA2001:AvoidCallingProblematicMethods",
            MessageId = "System.GC.Collect")]
        static void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
