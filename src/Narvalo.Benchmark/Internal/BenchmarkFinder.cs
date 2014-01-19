namespace Narvalo.Benchmark.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Linq;
    using Narvalo.Fx;

    class BenchmarkFinder
    {
        const BindingFlags DefaultBindings
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        readonly BindingFlags _bindings;

        public BenchmarkFinder() : this(DefaultBindings) { }

        public BenchmarkFinder(BindingFlags bindings)
        {
            _bindings = bindings;
        }

        public IEnumerable<Benchmark> FindBenchmarks(Assembly assembly)
        {
            Require.NotNull(assembly, "assembly");

            return from type in assembly.GetExportedTypes()
                   from benchmark in FindBenchmarks(type)
                   select benchmark;
        }

        public IEnumerable<Benchmark> FindBenchmarks(Type type)
        {
            Require.NotNull(type, "type");

            return Maybe.SelectAny(
                type.GetMethods(_bindings), 
                BenchmarkFactory.MayCreate);
        }
    }
}
