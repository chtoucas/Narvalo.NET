namespace Narvalo.ComparativeTests
{
    using System;
    using Narvalo.Benchmark;
    using Narvalo.ComparativeTests.Internal;
    using Narvalo.ComparativeTests.StringManip;

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
