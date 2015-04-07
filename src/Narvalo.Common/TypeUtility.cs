// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Reflection;
    using System.Diagnostics.Contracts;

    public static class TypeUtility
    {
        /// <summary>
        /// Returns a value indicating whether the specified type parameter is an enumeration.
        /// </summary>
        /// <typeparam name="T">The type to be checked.</typeparam>
        /// <returns><c>true</c> if the specified type parameter is an enumeration; 
        /// otherwise <c>false</c>.</returns>
        public static bool IsEnum<T>() where T : struct
        {
            return typeof(T).IsEnum;
        }

        /// <summary>
        /// Returns a value indicating whether the specified type parameter is a flags enumeration.
        /// </summary>
        /// <typeparam name="T">The type to be checked.</typeparam>
        /// <returns><c>true</c> if the specified type parameter is a flags enumeration; 
        /// otherwise <c>false</c>.</returns>
        public static bool IsFlagsEnum<T>() where T : struct
        {
            var type = typeof(T);

            return type.IsEnum && HasFlagsInternal(type);
        }

        /// <summary>
        /// Returns a value indicating whether the specified <paramref name="type"/> is a flags enumeration.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <returns><c>true</c> if the specified <paramref name="type"/> is a flags enumeration; 
        /// otherwise <c>false</c>.</returns>
        [Pure]
        public static bool IsFlagsEnum(Type type)
        {
            return type != null && type.IsEnum && HasFlagsInternal(type);
        }

        /// <summary>
        /// Returns a value indicating whether the specified type parameter is a value type.
        /// </summary>
        /// <typeparam name="T">The type to be checked.</typeparam>
        /// <returns><c>true</c> if the specified type parameter is a value type; 
        /// otherwise <c>false</c>.</returns>
        public static bool IsValueType<T>()
        {
            return typeof(T).IsValueType;
        }

        [Pure]
        internal static bool HasFlagsInternal(Type type)
        {
            Contract.Requires(type != null);

            var attribute = type.GetCustomAttribute<FlagsAttribute>(inherit: false);

            return attribute != null;
        }
    }
}
