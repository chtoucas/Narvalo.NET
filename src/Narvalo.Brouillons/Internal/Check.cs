// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    using Narvalo.Internal;

    [DebuggerStepThrough]
    internal static class Check
    {
        /// <summary>
        /// Asserts that the specified type parameter is an enumeration.
        /// </summary>
        /// <typeparam name="T">The type to be checked.</typeparam>
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void IsEnum<T>()
        {
            Contract.Requires(typeof(T).IsEnum);

            Debug.Assert(typeof(T).IsEnum, "The type is not an enumeration.");
        }

        /// <summary>
        /// Asserts that the specified type parameter is a value type.
        /// </summary>
        /// <typeparam name="T">The type to be checked.</typeparam>
        [ContractAbbreviator]
        [Conditional("DEBUG")]
        [Conditional("CONTRACTS_FULL")]
        public static void IsValueType<T>()
        {
            Contract.Requires(typeof(T).IsValueType);

            Debug.Assert(typeof(T).IsValueType, "The type is not a value type.");
        }
    }

    //    public static class TypeExtensions
    //    {
    //        /// <summary>
    //        /// Returns the <see cref="System.Reflection.TypeInfo"/> representation of the specified type.
    //        /// Workaround for the fact that IntrospectionExtensions.GetTypeInfo() does not have any contract attached.
    //        /// </summary>
    //        /// <remarks>This method MUST remain public in order to be used inside CC preconditions.</remarks>
    //        /// <param name="type">The type to convert.</param>
    //        /// <returns>The converted object.</returns>
    //        [Pure]
    //        public static TypeInfo GetTypeInfo(this Type @this)
    //        {
    //            Contract.Ensures(Contract.Result<TypeInfo>() != null);

    //            // NB: The properties Type.IsEnum and Type.IsValueType do not exist in the context of PCL.
    //            return @this.GetTypeInfo().AssumeNotNull();
    //        }
    //    }
}
