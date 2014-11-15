namespace Playground.Benchmark.Benchmarking.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Narvalo.Fx;
    using Playground.Benchmark.Benchmarking;

    static class BenchmarkComparisonFactory
    {
        public static BenchmarkComparison Create(Type type, IEnumerable<BenchmarkComparative> items)
        {
            return MayCreate(type, items)
                .ValueOrThrow(
                    () =>
                    {
                        var message = String.Format(
                            CultureInfo.CurrentCulture,
                            SR.MissingBenchComparisonAttribute,
                            type.Name);
                        return new BenchmarkException(message);
                    }
                );
        }

        public static Maybe<BenchmarkComparison> MayCreate(
            Type type,
            IEnumerable<BenchmarkComparative> items)
        {
            return from attr in type.MayGetCustomAttribute<BenchmarkComparisonAttribute>(inherit: false)
                   select new BenchmarkComparison(
                       GetName(type, attr),
                       items,
                       attr.Iterations);
        }

        #region Membres privés.

        static string GetName(Type type, BenchmarkComparisonAttribute attr)
        {
            return String.IsNullOrWhiteSpace(attr.DisplayName) ? type.Name : attr.DisplayName;
        }

        #endregion
    }
}
