// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo;

    public sealed class Benchmark
    {
        private readonly int _iterations;
        private readonly string _name;
        private readonly Action _action;

        public Benchmark(string name, Action action, int iterations)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(action, "action");

            _name = name;
            _action = action;
            _iterations = iterations;
        }

        public Action Action
        {
            get
            {
                Contract.Ensures(Contract.Result<Action>() != null);

                return _action;
            }
        }

        public int Iterations { get { return _iterations; } }

        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null); 

                return _name;
            }
        }

        public override string ToString()
        {
            return Name;
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_name != null);
            Contract.Invariant(_name.Length != 0);
            Contract.Invariant(_action != null);
        }

#endif
    }
}
