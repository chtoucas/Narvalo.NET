// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    public sealed class BenchmarkComparativeFinder
    {
        private const BindingFlags DEFAULT_BINDINGS
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly BindingFlags _bindings;

        public BenchmarkComparativeFinder() : this(DEFAULT_BINDINGS) { }

        public BenchmarkComparativeFinder(BindingFlags bindings)
        {
            _bindings = bindings;
        }

        public IEnumerable<BenchmarkComparative> FindComparatives(Type type)
        {
            Require.NotNull(type, "type");

            MethodInfo[] methods = type.GetMethods(_bindings);

            foreach (var method in methods)
            {
                var attr = Attribute.GetCustomAttribute(method, typeof(BenchmarkComparativeAttribute), inherit: false);

                if (attr == null)
                {
                    continue;
                }

                var benchAttr = attr as BenchmarkAttribute;

                yield return new BenchmarkComparative(
                    benchAttr.DisplayName ?? method.Name,
                    ActionFactory.Create(method));
            }
        }

        // FIXME: Theory.
        public IEnumerable<BenchmarkComparative> FindComparatives<T>(Type type, T value)
        {
            Require.NotNull(type, "type");

            MethodInfo[] methods = type.GetMethods(_bindings);

            foreach (var method in methods)
            {
                var attr = Attribute.GetCustomAttribute(method, typeof(BenchmarkComparativeAttribute), inherit: false);

                if (attr == null)
                {
                    continue;
                }

                var benchAttr = attr as BenchmarkComparativeAttribute;

                yield return new BenchmarkComparative(
                    benchAttr.DisplayName ?? method.Name,
                    () => ActionFactory.Create<T>(method).Invoke(value));
            }
        }

        //public IEnumerable<BenchmarkComparative> FindComparatives(Type type)
        //{
        //    Require.NotNull(type, "type");

        //    return type.GetMethods(_bindings).MapAny(MaySelectBenchmarkComparative_);
        //}

        //public IEnumerable<BenchmarkComparative> FindComparatives<T>(Type type, T value)
        //{
        //    Require.NotNull(type, "type");

        //    return type.GetMethods(_bindings).MapAny(_ => MaySelectBenchmarkComparative_(_, value));
        //}

        //private static Maybe<BenchmarkComparative> MaySelectBenchmarkComparative_(MethodInfo method)
        //{
        //    return from attr in method.MayGetBenchmarkComparisonAttribute()
        //           select new BenchmarkComparative(
        //               attr.DisplayName ?? method.Name,
        //               ActionFactory.Create(method));
        //}

        //// FIXME: Theory.
        //private static Maybe<BenchmarkComparative> MaySelectBenchmarkComparative_<T>(MethodInfo method, T value)
        //{
        //    return from attr in method.MayGetBenchmarkComparisonAttribute()
        //           select new BenchmarkComparative(
        //               attr.DisplayName ?? method.Name,
        //               () => ActionFactory.Create<T>(method).Invoke(value));
        //}
    }
}
