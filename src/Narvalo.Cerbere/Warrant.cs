// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public static class Warrant
    {
        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] Abbreviator for Contract.Result<T>().")]
        public static void NotNull<T>() where T : class
            => Contract.Ensures(Contract.Result<T>() != null);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] Abbreviator for Contract.Result<T>().")]
        public static void NotNullUnconstrained<T>()
            => Contract.Ensures(Contract.Result<T>() != null);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void NotNullOrEmpty()
        {
            // TODO: Explain why we do not use String.IsNullOrEmpty().
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length != 0);
        }

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void IsTrue()
            => Contract.Ensures(Contract.Result<bool>() == true);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void IsFalse()
            => Contract.Ensures(Contract.Result<bool>() == false);

        [DebuggerHidden]
        [ContractAbbreviator]
        [Conditional("CONTRACTS_FULL")]
        [ExcludeFromCodeCoverage]
        public static void PassThrough<T>(T @this) where T : class
            => Contract.Ensures(Contract.Result<T>() == @this);
    }
}
