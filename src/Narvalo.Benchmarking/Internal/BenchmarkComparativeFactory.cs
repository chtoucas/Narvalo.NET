// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking.Internal
{
    using System;
    using System.Reflection;

    using Narvalo.Benchmarking;
    using Narvalo.Fx;

    internal static class BenchmarkComparativeFactory
    {
        ////public static BenchComparative Create(MethodInfo method)
        ////{
        ////    return MayCreate(method)
        ////        .ValueOrThrow(GetExceptionThunk(method.Name));
        ////}

        //// FIXME: Theory.
        ////public static BenchComparative Create<T>(MethodInfo method, T value)
        ////{
        ////    return MayCreate(method, value)
        ////        .ValueOrThrow(GetExceptionThunk(method.Name));
        ////}

        public static Maybe<BenchmarkComparative> MayCreate(MethodInfo method)
        {
            return from attr in MayGetAttribute(method)
                   select new BenchmarkComparative(
                       GetName(method, attr),
                       ActionFactory.Create(method));
        }

        // FIXME: Theory.
        public static Maybe<BenchmarkComparative> MayCreate<T>(MethodInfo method, T value)
        {
            return from attr in MayGetAttribute(method)
                   select new BenchmarkComparative(
                       GetName(method, attr),
                       () => ActionFactory.Create<T>(method).Invoke(value));
        }

        ////private static Func<BenchException> GetExceptionThunk(string name)
        ////{
        ////    return () => {
        ////        var message = String.Format(
        ////            CultureInfo.CurrentCulture,
        ////            Strings_Benchmarking.MissingBenchComparativeAttribute,
        ////            name);
        ////        return new BenchException(message);
        ////    };
        ////}

        private static string GetName(MethodInfo method, BenchmarkComparativeAttribute attr)
        {
            return String.IsNullOrWhiteSpace(attr.DisplayName) ? method.Name : attr.DisplayName;
        }

        private static Maybe<BenchmarkComparativeAttribute> MayGetAttribute(MethodInfo method)
        {
            return method
                .MayGetCustomAttribute<BenchmarkComparativeAttribute>(false /* inherit */);
        }
    }
}
