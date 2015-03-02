// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking.Internal
{
    using System;
    using System.Reflection;

    using Narvalo.Benchmarking;
    using Narvalo.Fx;

    internal static class BenchmarkFactory
    {
        ////public static Benchmark Create(MethodInfo method)
        ////{
        ////    return MayCreate(method)
        ////        .ValueOrThrow(
        ////            () => {
        ////                var message = String.Format(
        ////                    CultureInfo.CurrentCulture,
        ////                    Strings_Benchmarking.MissingBenchmarkAttribute,
        ////                    method.Name);
        ////                return new BenchException(message);
        ////            }
        ////        );
        ////}

        public static Maybe<Benchmark> MayCreate(MethodInfo method)
        {
            return from attr in method.MayGetCustomAttribute<BenchmarkAttribute>(false /* inherit */)
                   select new Benchmark(
                       GetName_(method, attr),
                       ActionFactory.Create(method),
                       attr.Iterations);
        }

        private static string GetName_(MethodInfo method, BenchmarkAttribute attr)
        {
            return String.IsNullOrWhiteSpace(attr.DisplayName) ? method.Name : attr.DisplayName;
        }
    }
}
