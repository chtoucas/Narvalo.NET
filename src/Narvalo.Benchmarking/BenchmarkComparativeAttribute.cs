// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BenchmarkComparativeAttribute : Attribute
    {
        private string _displayName = String.Empty;

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
