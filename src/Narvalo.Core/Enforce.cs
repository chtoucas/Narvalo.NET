// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif

    /// <summary>
    /// Provides helper methods to write assertions on arguments. 
    /// These methods should only describe conditions that you know will ALWAYS be true.
    /// In Release Build, these methods are erased, unless you turn on Code Contracts,
    /// in which case we will even try to prove the assertion.
    /// </summary>
    [DebuggerStepThrough]
    public static class Enforce
    {
#if CONTRACTS_FULL
        [ContractAbbreviator]
        public static void NotNull<T>(T value, string parameterName) where T : class
        {
            Contract.Requires(value != null, Format.CurrentCulture(SR.Enforce_IsNullFormat, parameterName));
        }

        [ContractAbbreviator]
        public static void NotNull<T>(T? value, string parameterName) where T : struct
        {
            Contract.Requires(value != null, Format.CurrentCulture(SR.Enforce_IsNullFormat, parameterName));
        }
#else
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value, string parameterName) where T : class
        {
            Debug.Assert(value != null, Format.CurrentCulture(SR.Enforce_IsNullFormat, parameterName));
        }

        [Conditional("DEBUG")]
        public static void NotNull<T>(T? value, string parameterName) where T : struct
        {
            Debug.Assert(value != null, Format.CurrentCulture(SR.Enforce_IsNullFormat, parameterName));
        }
#endif
    }
}
