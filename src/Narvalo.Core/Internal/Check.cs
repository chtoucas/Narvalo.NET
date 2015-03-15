// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides helper methods to assert conditions on arguments, ie describe situations 
    /// that you know should ALWAYS be true.
    /// If Code Contracts is enabled the methods are recognized as preconditions.
    /// If Code Contracts is disabled,
    /// - In Debug builds, the methods turn into assertions.
    /// - Otherwise, the methods are simply erased by the compiler.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Check
    {
        /// <summary>
        /// Asserts that the specified argument is not <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="value"/>.</typeparam>
        /// <param name="value">The argument to check.</param>
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        [ContractAbbreviator]
        public static void NotNull<T>(T value) where T : class
        {
#if CONTRACTS_FULL
            Contract.Requires(value != null);
#else
            Debug.Assert(value != null);
#endif
        }
    }
}
