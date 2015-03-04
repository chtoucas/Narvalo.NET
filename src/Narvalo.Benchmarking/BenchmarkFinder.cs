// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    public sealed class BenchmarkFinder
    {
        private readonly BindingFlags _bindings;

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

            MethodInfo[] methods = type.GetMethods(_bindings);

            foreach (var method in methods)
            {
                BenchmarkAttribute attr
                    = method.GetCustomAttribute<BenchmarkAttribute>(inherit: false);

                if (attr == null)
                {
                    continue;
                }

                yield return new Benchmark(
                    attr.DisplayName ?? method.Name,
                    ActionFactory.Create(method),
                    attr.Iterations);
            }
        }
    }
}
