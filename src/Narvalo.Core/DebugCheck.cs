// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
#if CONTRACTS_FULL
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Narvalo.Internal;
#else
    using System.Diagnostics;
    using Narvalo.Internal;
#endif

    // https://social.msdn.microsoft.com/Forums/en-US/434fdb77-262e-474b-84ee-1fb0d5a843b1/any-way-to-bypass-validation-on-overload-methods?forum=codecontracts

    [DebuggerStepThrough]
    public static class Check
    {
#if CONTRACTS_FULL
        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value)
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            Contract.EndContractBlock();
        }
#else
        [Conditional("DEBUG")]
        public static void NotNull<T>([ValidatedNotNull]T value)
        {
            Debug.Assert(value != null, SR.DebugCheck_IsNull);
        }
#endif

#if CONTRACTS_FULL
        [ContractArgumentValidator]
        public static void NotNullOrEmpty([ValidatedNotNull]string value)
        {
            NotNull(value);

            if (value.Length == 0) {
                throw new ArgumentException(Format.CurrentCulture(SR.Require_ArgumentEmptyFormat, "value"), "value");
            }

            Contract.EndContractBlock();
        }
#else
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty([ValidatedNotNull]string value)
        {
            NotNull(value);
            Debug.Assert(value.Length != 0, SR.DebugCheck_IsEmpty);
        }
#endif
    }

    [DebuggerStepThrough]
    public static class DebugCheck
    {
#if CONTRACTS_FULL
        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value)
        {
            if (value == null) {
                throw ExceptionFactory.ArgumentNull("value");
            }

            Contract.EndContractBlock();
        }
#else
        [Conditional("DEBUG")]
        public static void NotNull<T>([ValidatedNotNull]T value)
        {
            Debug.Assert(value != null, SR.DebugCheck_IsNull);
        }
#endif

#if CONTRACTS_FULL
        [ContractArgumentValidator]
        public static void NotNullOrEmpty([ValidatedNotNull]string value)
        {
            NotNull(value);

            if (value.Length == 0) {
                throw new ArgumentException(Format.CurrentCulture(SR.Require_ArgumentEmptyFormat, "value"), "value");
            }

            Contract.EndContractBlock();
        }
#else
        [Conditional("DEBUG")]
        public static void NotNullOrEmpty([ValidatedNotNull]string value)
        {
            NotNull(value);
            Debug.Assert(value.Length != 0, SR.DebugCheck_IsEmpty);
        }
#endif
    }
}
