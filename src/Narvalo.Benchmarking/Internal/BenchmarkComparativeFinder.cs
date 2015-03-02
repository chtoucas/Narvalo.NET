// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Narvalo;
    using Narvalo.Benchmarking;
    using Narvalo.Collections;

    internal sealed class BenchmarkComparativeFinder
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

            return type.GetMethods(_bindings).MapAny(BenchmarkComparativeFactory.MayCreate);
        }

        public IEnumerable<BenchmarkComparative> FindComparatives<T>(Type type, T value)
        {
            Require.NotNull(type, "type");

            return type.GetMethods(_bindings).MapAny(_ => BenchmarkComparativeFactory.MayCreate(_, value));
        }
    }
}
