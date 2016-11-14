// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerStepThrough]
    public static class Expect
    {
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void State(bool testCondition) => True(testCondition);

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void True(bool testCondition) => Contract.Requires(testCondition);

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void Range(bool rangeCondition) => True(rangeCondition);

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void NotNull<T>(T value) where T : class => True(value != null);

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void NotNullOrEmpty(string value) => True(!String.IsNullOrEmpty(value));

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void NotNullOrWhiteSpace(string value) => True(!String.IsNullOrWhiteSpace(value));

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void Object<T>(T @this) where T : class => True(@this != null);

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void Property(bool testCondition) => True(testCondition);

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void Property<T>(T value) where T : class => True(value != null);

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void PropertyNotEmpty(string value) => True(!String.IsNullOrEmpty(value));

        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(
            Justification = "OpenCover can't discover the tests because of the CONTRACTS_FULL conditional.")]
        public static void PropertyNotWhiteSpace(string value) => True(!String.IsNullOrWhiteSpace(value));
    }
}
