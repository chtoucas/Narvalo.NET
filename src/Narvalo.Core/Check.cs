// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif

    [DebuggerStepThrough]
    public static class Check
    {
#if CONTRACTS_FULL
        [ContractAbbreviator]
        public static void NotNull<T>(T value)
        {
            Contract.Requires(value != null);
        }
        
        [ContractAbbreviator]
        public static void NotNullOrEmpty(string value)
        {
            NotNull(value);
            Contract.Requires(value.Length != 0);
        }
#else
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value)
        {
            Debug.Assert(value != null, SR.Check_IsNull);
        }

        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value)
        {
            NotNull(value);
            Debug.Assert(value.Length != 0, SR.Check_IsEmpty);
        }
#endif
    }
}
