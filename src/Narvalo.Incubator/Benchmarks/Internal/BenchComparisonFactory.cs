namespace Narvalo.Benchmarks.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Narvalo.Fx;

    internal static class BenchComparisonFactory
    {
        public static BenchComparison Create(Type type, IEnumerable<BenchComparative> items)
        {
            return MayCreate(type, items)
                .ValueOrThrow(
                    () => {
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
            return type
                .MayGetCustomAttribute<BenchComparisonAttribute>(false /* inherit */)
                .Map(
                    attr => new BenchComparison(
                        GetName(type, attr),
                        items,
                        attr.Iterations)
                );
        }

        #region Membres privés.

        static string GetName(Type type, BenchComparisonAttribute attr)
        {
            return String.IsNullOrWhiteSpace(attr.DisplayName) ? type.Name : attr.DisplayName;
        }

        #endregion
    }
}
