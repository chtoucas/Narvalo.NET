// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Narvalo.Benchmarking;
    using Narvalo.Fx;

    internal static class BenchmarkComparisonFactory
    {
        public static BenchmarkComparison Create(Type type, IEnumerable<BenchmarkComparative> items)
        {
            return MayCreate(type, items)
                .ValueOrThrow(
                    () => {
                        var message = String.Format(
                            CultureInfo.CurrentCulture,
                            Strings_Benchmarking.MissingBenchComparisonAttribute,
                            type.Name);
                        return new BenchmarkException(message);
                    });
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

        private static string GetName(Type type, BenchmarkComparisonAttribute attr)
        {
            return String.IsNullOrWhiteSpace(attr.DisplayName) ? type.Name : attr.DisplayName;
        }
    }
}
