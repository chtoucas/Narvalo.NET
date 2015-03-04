// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Benchmarks.Sdk
{
    using System;

    using Narvalo;

    public sealed class BenchmarkComparative
    {
        private readonly string _name;
        private readonly Action _action;

        public BenchmarkComparative(string name, Action action)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(action, "action");

            _name = name;
            _action = action;
        }

        public Action Action { get { return _action; } }

        public string Name { get { return _name; } }

        public override string ToString()
        {
            return Name;
        }
    }
}
