// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Narvalo;
    using Narvalo.Benchmarking;
    using Narvalo.Collections;

    internal sealed class BenchmarkFinder
    {
        private const BindingFlags DEFAULT_BINDINGS
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly BindingFlags _bindings;

        public BenchmarkFinder() : this(DEFAULT_BINDINGS) { }

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

            return type.GetMethods(_bindings).MapAny(BenchmarkFactory.MayCreate);
        }
    }
}
