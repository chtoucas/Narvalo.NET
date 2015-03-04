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
                BenchmarkComparativeAttribute attr 
                    = method.GetCustomAttribute<BenchmarkComparativeAttribute>(inherit: false);

                if (attr == null)
                {
                    continue;
                }

                yield return new BenchmarkComparative(
                    attr.DisplayName ?? method.Name,
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
                BenchmarkComparativeAttribute attr 
                    = method.GetCustomAttribute<BenchmarkComparativeAttribute>(inherit: false);

                if (attr == null)
                {
                    continue;
                }

                yield return new BenchmarkComparative(
                    attr.DisplayName ?? method.Name,
                    () => ActionFactory.Create<T>(method).Invoke(value));
            }
        }
    }
}
