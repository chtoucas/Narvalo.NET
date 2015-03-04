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

        public BenchmarkRunner(IBenchmarkTimer timer)
        {
            Require.NotNull(timer, "timer");

            _timer = timer;
        }

        public Duration Run(Benchmark benchmark)
        {
            Require.NotNull(benchmark, "benchmark");

            return Time(benchmark.Action, benchmark.Iterations);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect",
            Justification = "The call to GC methods is done on purpose to ensure timing happens in a clean room.")]
        public Duration Time(Action action, int iterations)
        {
            Require.NotNull(action, "action");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Warmup (To be improved).
            action();

            _timer.Reset();

            for (int i = 0; i < iterations; i++) {
                action();
            }

            return _timer.ElapsedTime;
        }
    }
}
