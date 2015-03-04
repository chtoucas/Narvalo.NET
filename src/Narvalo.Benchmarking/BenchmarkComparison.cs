// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    public sealed class BenchmarkComparison
    {
        private readonly IEnumerable<BenchmarkComparative> _items;
        private readonly int _iterations;
        private readonly string _name;

        public BenchmarkComparison(
            string name,
            IEnumerable<BenchmarkComparative> items,
            int iterations)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(items, "items");

            _name = name;
            _items = items;
            _iterations = iterations;
        }

        public IEnumerable<BenchmarkComparative> Items { get { return _items; } }

        public int Iterations { get { return _iterations; } }

        public string Name { get { return _name; } }

        internal static BenchmarkComparison Create(Type type, IEnumerable<BenchmarkComparative> items)
        {
            var attr = Attribute.GetCustomAttribute(type, typeof(BenchmarkComparisonAttribute), inherit: false);

            if (attr == null)
            {
                var message = String.Format(
                    CultureInfo.CurrentCulture,
                    Strings_Benchmarking.MissingBenchComparisonAttribute,
                    type.Name);

                throw new BenchmarkException(message);
            }

            var benchAttr = attr as BenchmarkComparisonAttribute;

            return new BenchmarkComparison(
                benchAttr.DisplayName ?? type.Name,
                items,
                benchAttr.Iterations);

            //var q = from attr in type.MayGetBenchmarkComparisonAttribute()
            //        select new BenchmarkComparison(
            //            attr.DisplayName ?? type.Name,
            //            items,
            //            attr.Iterations);

            //return q.ValueOrThrow(
            //    () =>
            //    {
            //        var message = String.Format(
            //            CultureInfo.CurrentCulture,
            //            Strings_Benchmarking.MissingBenchComparisonAttribute,
            //            type.Name);
            //        return new BenchmarkException(message);
            //    });
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
