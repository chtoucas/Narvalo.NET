namespace Playground.Benchmark.Benchmarking.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Collections;
    using Playground.Benchmark.Benchmarking;

    class BenchmarkComparativeFinder
    {
        const BindingFlags DefaultBindings
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        readonly BindingFlags _bindings;

        public BenchmarkComparativeFinder() : this(DefaultBindings) { }

        public BenchmarkComparativeFinder(BindingFlags bindings)
        {
            _bindings = bindings;
        }

        public IEnumerable<BenchmarkComparative> FindComparatives(Type type)
        {
            Require.NotNull(type, "type");

            return type.GetMethods(_bindings).MapAny(BenchmarkComparativeFactory.MayCreate);
        }

        public IEnumerable<BenchmarkComparative> FindComparatives<T>(Type type, T value)
        {
            Require.NotNull(type, "type");

            return type.GetMethods(_bindings).MapAny(_ => BenchmarkComparativeFactory.MayCreate(_, value));
        }
    }
}
