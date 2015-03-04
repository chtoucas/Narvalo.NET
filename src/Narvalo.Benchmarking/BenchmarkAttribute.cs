// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BenchmarkAttribute : Attribute
    {
        private readonly int _iterations;

        public BenchmarkAttribute(int iterations)
        {
            Require.GreaterThanOrEqualTo(iterations, 1, "iterations");

            _iterations = iterations;
        }

        public string DisplayName { get; set; }

        public int Iterations
        {
            get { return _iterations; }
        }
    }
}
