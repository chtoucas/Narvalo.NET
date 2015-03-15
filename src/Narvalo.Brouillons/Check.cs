// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    using Narvalo.Internal;

    /// <summary>
    /// Provides helper methods to write assertions. 
    /// These methods should only describe conditions that you know will ALWAYS be true.
    /// In Release configuration, these methods are simply erased. 
    /// </summary>
    [DebuggerStepThrough]
    public static class CheckXXX
    {
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void IsEnum(Type type)
        {
            Contract.Requires(type != null);
            // NB: type.IsEnum is not available in the context of PCL.
            Contract.Requires(!GetTypeInfo(type).IsEnum);
        }

        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void IsValueType(Type type)
        {
            Contract.Requires(type != null);
            // NB: type.IsValueType is not available in the context of PCL.
            Contract.Requires(!GetTypeInfo(type).IsValueType);
        }

        //[Conditional("DEBUG")]
        //public static void IsEnum(Type type)
        //{
        //    Contract.Requires(type != null);

        //    Debug.Assert(GetTypeInfo_(type).IsEnum, Format.CurrentCulture("The type '{0}' MUST be of enum type.", type.FullName));

        //    //// NB: type.IsEnum is not available in the context of PCL.
        //    ////if (!typeof(Enum).GetTypeInfo().AssumeNotNull().IsAssignableFrom(type.GetTypeInfo()))
        //    //if (!GetTypeInfo(type).IsEnum)
        //    //{
        //    //    throw new ArgumentException(Format.CurrentCulture("The type '{0}' MUST be of enum type.", type.FullName));
        //    //}

        //    //Contract.EndContractBlock();
        //}

        //[Conditional("DEBUG")]
        //public static void IsValueType(Type type)
        //{
        //    Contract.Requires(type != null);

        //    //// NB: type.IsValueType is not available in the context of PCL.
        //    Debug.Assert(GetTypeInfo_(type).IsValueType, Format.CurrentCulture("The type '{0}' MUST be of value type.", type.FullName));
        //}

        //[Conditional("DEBUG")]
        //public static void NotNull<T>(T? value, string parameterName) where T : struct
        //{
        //    Debug.Assert(value != null, Format.CurrentCulture("The parameter '{0}' MUST NOT be null.", parameterName));
        //}

        /// <summary>
        /// Returns the System.Reflection.TypeInfo representation of the specified type.
        /// Workaround for the fact that IntrospectionExtensions.GetTypeInfo() does not have any contract attached.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <returns>The converted object.</returns>
        [Pure]
        public static TypeInfo GetTypeInfo(Type type)
        {
            Contract.Ensures(Contract.Result<TypeInfo>() != null);

            return type.GetTypeInfo().AssumeNotNull();
        }
    }
}
