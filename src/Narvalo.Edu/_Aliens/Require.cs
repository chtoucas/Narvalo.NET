// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Narvalo.Internal;

    static class Require
    {
        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void Object<T>([ValidatedNotNull]T @this) where T : class
        {
            if (@this == null) {
                throw new ArgumentNullException("this");
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static T Property<T>([ValidatedNotNull]T value) where T : class
        {
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            Contract.EndContractBlock();

            return value;
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void NotNull<T>([ValidatedNotNull]T value, string parameterName) where T : class
        {
            if (value == null) {
                throw new ArgumentNullException(parameterName);
            }

            Contract.EndContractBlock();
        }

        [DebuggerStepThrough]
        [ContractArgumentValidator]
        public static void NotNullOrEmpty([ValidatedNotNull]string value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Length == 0) {
                throw new ArgumentException(parameterName);
            }

            Contract.EndContractBlock();
        }
    }
}