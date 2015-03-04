// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo;
    using Narvalo.Benchmarking;
    using NodaTime;

    public sealed class Benchmarker
    {
        private readonly IBenchmarkTimer _timer;

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

            Cleanup_();

            // Warmup (To be improved).
            action();

            _timer.Reset();

            for (int i = 0; i < iterations; i++) {
                action();
            }

            return _timer.ElapsedTime;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect",
            Justification = "The call to GC methods is done on purpose to ensure timing happens in a clean room.")]
        private static void Cleanup_()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
