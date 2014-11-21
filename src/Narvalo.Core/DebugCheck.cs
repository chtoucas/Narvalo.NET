// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
#if CONTRACTS_FULL
    using System;
#endif
    using System.Diagnostics;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif
    using Narvalo.Internal;

    // https://social.msdn.microsoft.com/Forums/en-US/434fdb77-262e-474b-84ee-1fb0d5a843b1/any-way-to-bypass-validation-on-overload-methods?forum=codecontracts
    [DebuggerStepThrough]
    public static class DebugCheck
    {
#if CONTRACTS_FULL
        [ContractArgumentValidator]
#else
        [Conditional("DEBUG")]
#endif
        public static void NotNull<T>(T value)
        {
#if CONTRACTS_FULL
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            Contract.EndContractBlock();
#else
            Debug.Assert(value != null, SR.DebugCheck_IsNull);
#endif
        }

#if CONTRACTS_FULL
        [ContractArgumentValidator]
#else
        [Conditional("DEBUG")]
#endif
        public static void NotNullOrEmpty(string value)
        {
#if CONTRACTS_FULL
            NotNull(value);

            if (value.Length == 0) {
                throw new ArgumentException(Format.CurrentCulture(SR.Require_ArgumentEmptyFormat, "value"), "value");
            }

            Contract.EndContractBlock();
#else
            NotNull(value);
            Debug.Assert(value.Length != 0, SR.DebugCheck_IsEmpty);
#endif
        }
    }
}
