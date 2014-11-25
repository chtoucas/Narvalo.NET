// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides helper methods to perform argument validation.
    /// If Code Contracts is enabled, these methods will be understood as Preconditions.
    /// It complements <see cref="Require" /> with methods not available with PCL.
    /// </summary>
    [DebuggerStepThrough]
    public static class Check
    {
        [ContractArgumentValidator]
        public static void IsEnum(Type type)
        {
            Require.NotNull(type, "type");

            if (!type.IsEnum) {
                throw new InvalidOperationException(Format.CurrentCulture(SR.Check_IsNotEnum, type.FullName ?? "Unknown type name"));
            }

            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void IsValueType(Type type)
        {
            Require.NotNull(type, "type");

            if (!type.IsValueType) {
                throw new InvalidOperationException(Format.CurrentCulture(SR.Check_IsNotValueType, type.FullName ?? "Unknown type name"));
            }

            Contract.EndContractBlock();
        }
    }
}
