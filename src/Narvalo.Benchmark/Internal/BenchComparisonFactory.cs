namespace Narvalo.Benchmark.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Narvalo.Fx;

    static class BenchComparisonFactory
    {
        public static BenchComparison Create(Type type, IEnumerable<BenchComparative> items)
        {
            return MayCreate(type, items)
                .ValueOrThrow(
                    () =>
                    {
                        var message = String.Format(
                            CultureInfo.CurrentCulture,
                            SR.MissingBenchComparisonAttribute,
                            type.Name);
                        return new BenchException(message);
                    }
                );
        }

        public static Maybe<BenchComparison> MayCreate(
            Type type,
            IEnumerable<BenchComparative> items)
        {
            return from attr in type.MayGetCustomAttribute<BenchComparisonAttribute>(inherit: false)
                   select new BenchComparison(
                       GetName(type, attr),
                       items,
                       attr.Iterations);
        }

        #region Membres privés.

        static string GetName(Type type, BenchComparisonAttribute attr)
        {
            return String.IsNullOrWhiteSpace(attr.DisplayName) ? type.Name : attr.DisplayName;
        }

        #endregion
    }
}
