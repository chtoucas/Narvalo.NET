namespace Narvalo.Benchmarking.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo;
    using Narvalo.Benchmarking;
    using NodaTime;

    class Benchmarker
    {
        readonly IBenchmarkTimer _timer;

        public Benchmarker(IBenchmarkTimer timer)
        {
            Require.NotNull(timer, "timer");

            _timer = timer;
        }

        public Duration Time(Benchmark benchmark)
        {
            Require.NotNull(benchmark, "benchmark");

            return Time(benchmark.Action, benchmark.Iterations);
        }

        public Duration Time(Action action, int iterations)
        {
            Require.NotNull(action, "action");

            Cleanup();

            // Echauffement (à améliorer).
            action();

            _timer.Reset();

            for (int i = 0; i < iterations; i++) {
                action();
            }

            return _timer.ElapsedTime;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect",
            Justification = "The call to GC methods is done on purpose to ensure timing happens in a clean room.")]
        static void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
