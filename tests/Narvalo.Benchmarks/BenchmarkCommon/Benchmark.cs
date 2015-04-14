// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.BenchmarkCommon
{
    using System;
    using System.Diagnostics.Contracts;

    public sealed class Benchmark
    {
        private readonly string _categoryName;
        private readonly string _name;
        private readonly Action _action;

        public Benchmark(string categoryName, string name, Action action)
        {
            Require.NotNullOrEmpty(categoryName, "categoryName");
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(action, "action");

            _categoryName = categoryName;
            _name = name;
            _action = action;
        }

        public Action Action
        {
            get
            {
                Contract.Ensures(Contract.Result<Action>() != null);

                return _action;
            }
        }

        public string CategoryName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(Contract.Result<string>().Length != 0);

                return _categoryName;
            }
        }

        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(Contract.Result<string>().Length != 0);

                return _name;
            }
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return "[" + CategoryName + "] " + Name;
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_categoryName != null);
            Contract.Invariant(_categoryName.Length != 0);
            Contract.Invariant(_name != null);
            Contract.Invariant(_name.Length != 0);
            Contract.Invariant(_action != null);
        }

#endif
    }
}
