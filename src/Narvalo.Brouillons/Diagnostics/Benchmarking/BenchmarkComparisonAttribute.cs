// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class BenchmarkComparisonAttribute : Attribute
    {
        private readonly int _iterations;

        public BenchmarkComparisonAttribute(int iterations)
        {
            _iterations = iterations;
        }

        public string DisplayName { get; set; }

        public int Iterations { get { return _iterations; } }
    }
}
