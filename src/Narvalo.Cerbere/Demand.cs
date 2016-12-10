// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    ///// <summary>
    ///// Provides helper methods to specify preconditions on a method.
    ///// </summary>
    ///// <remarks>
    ///// <para>The methods WON'T be recognized by FxCop as guards against <see langword="null"/> value.</para>
    ///// <para>If a condition does not hold, an unrecoverable exception is thrown
    ///// in debug builds.</para>
    ///// <para>This class MUST NOT be used in place of proper validation of public
    ///// arguments but is only useful in very specialized use cases. Be wise.</para>
    ///// <para>Personally, I can only see one situation where these helpers make sense:
    ///// for protected overridden methods in a sealed class when the base method
    ///// declares a contract (otherwise you should use <see cref="Promise"/>),
    ///// when you know for certain that all callers will satisfy the condition
    ///// and most certainly when you own all base classes. As you can see, that
    ///// makes a lot of prerequisites...
    ///// </para>
    ///// </remarks>
    [DebuggerStepThrough]
    public static class Demand
    {
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void State(bool testCondition) => True(testCondition);

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
        public static void Range(bool rangeCondition) => True(rangeCondition);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNull<T>(T value) where T : class => True(value != null);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullUnconstrained<T>(T value) => True(value != null);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrEmpty(string value) => True(!String.IsNullOrEmpty(value));

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void NotNullOrWhiteSpace(string value)
        {
            // Do not use String.IsNullOrWhiteSpace(), it does not work with CCCheck.
            True(!String.IsNullOrEmpty(value));
            True(!Enforce.IsWhiteSpace(value));
        }

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void Object<T>(T @this) where T : class => True(@this != null);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Demand.NotNullUnconstrained() instead.", true)]
        public static void Property<T>(T value) => NotNullUnconstrained(value);

        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Demand.NotNullOrEmpty() instead.", true)]
        public static void PropertyNotEmpty(string value) => True(!String.IsNullOrEmpty(value));
    }
}
