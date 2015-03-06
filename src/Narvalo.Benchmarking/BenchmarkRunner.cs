// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo;
    using NodaTime;

    public sealed class BenchmarkRunner
    {
        private readonly IBenchmarkTimer _timer;

        private Duration _testDuration = Duration.FromSeconds(10);
        private Duration _warmUpDuration = Duration.FromSeconds(1);

        public BenchmarkRunner(IBenchmarkTimer timer)
        {
            Require.NotNull(timer, "timer");

            _timer = timer;
        }

        public Duration TestDuration
        {
            get { return _testDuration; }
            set { _testDuration = value; }
        }

        public Duration WarmUpDuration
        {
            get { return _warmUpDuration; }
            set { _warmUpDuration = value; }
        }

        // Constant time benchmarking.
        public BenchmarkMetric Run(Benchmark benchmark)
        {
            Require.NotNull(benchmark, "benchmark");

            int iterations = WarmUp_(benchmark.Action);
            Duration duration = Time_(benchmark.Action, iterations);

            return new BenchmarkMetric(benchmark.CategoryName, benchmark.Name, duration, iterations);
        }

        // Benchmark for a pre-defined number of iterations.
        public BenchmarkMetric Run(Benchmark benchmark, int iterations)
        {
            Require.NotNull(benchmark, "benchmark");

            // Warmup.
            benchmark.Action();

            Duration duration = Time_(benchmark.Action, iterations);

            return new BenchmarkMetric(benchmark.CategoryName, benchmark.Name, duration, iterations, fixedTime: false);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect",
            Justification = "The call to GC methods is done on purpose to ensure timing happens in a clean room.")]
        private Duration Time_(Action action, int iterations)
        {
            // Make sure we start with a clean room.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            _timer.Reset();

            for (int i = 0; i < iterations; i++)
            {
                action();
            }

            return _timer.ElapsedTime;
        }

        private int WarmUp_(Action action)
        {
            int iterations = 100;
            while (true)
            {
                Duration duration = Time_(action, iterations);
                if (duration >= WarmUpDuration)
                {
                    double scale = ((double)TestDuration.Ticks) / duration.Ticks;
                    iterations = (int)Math.Min(scale * iterations, int.MaxValue - 1);
                    break;
                }

                if (iterations >= int.MaxValue / 2)
                {
                    break;
                }

                iterations *= 2;
            }

            return iterations;
        }
    }
}
