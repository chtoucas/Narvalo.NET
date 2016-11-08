// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerStepThrough]
    public static class Demand
    {
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void True(bool testCondition)
        {
            Contract.Requires(testCondition);
            Debug.Assert(testCondition);
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Object<T>(T @this) where T : class
        {
            Contract.Requires(@this != null);
            Debug.Assert(@this != null);
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Property<T>(T value) where T : class
        {
            Contract.Requires(value != null);
            Debug.Assert(value != null);
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void PropertyNotEmpty(string value)
        {
            Contract.Requires(!String.IsNullOrEmpty(value));
            Debug.Assert(!String.IsNullOrEmpty(value));
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void PropertyNotWhiteSpace(string value)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(value));
            Debug.Assert(!String.IsNullOrWhiteSpace(value));
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNull<T>(T value) where T : class
        {
            Contract.Requires(value != null);
            Debug.Assert(value != null);
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrEmpty(string value)
        {
            Contract.Requires(!String.IsNullOrEmpty(value));
            Debug.Assert(!String.IsNullOrEmpty(value));
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrWhiteSpace(string value)
        {
            Contract.Requires(!String.IsNullOrWhiteSpace(value));
            Debug.Assert(!String.IsNullOrWhiteSpace(value));
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Range(bool rangeCondition)
        {
            Contract.Requires(rangeCondition);
            Debug.Assert(rangeCondition);
        }
    }
}
