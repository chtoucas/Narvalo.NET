namespace Playground.Benchmark
{
    using System;
    using Narvalo.Benchmark;
    using Playground.Benchmark.Internal;
    using Playground.Benchmark.StringManip;

    class Program
    {
        static void Main()
        {
            var metrics = BenchComparisonProcessor
                .Create()
                .Process(typeof(RemoveDiacriticsComparison));

            var fmt = new BenchMetricConsoleFormatter();

            Console.WriteLine(metrics.ToString(fmt));
        }
    }
}
